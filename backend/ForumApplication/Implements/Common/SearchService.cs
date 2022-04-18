using EnsureThat;
using ForumApplication.Dtos.Search;
using ForumApplication.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Dynamic.Core;
using AutoMapper;

namespace ForumApplication.Implements.Common
{
    public abstract class SearchService<TEntity, TModel, TKey> : ISearchService<TEntity, TModel, TKey> where TEntity : class where TModel : class
    {
        private readonly IMapper _mapper;

        protected IServiceProvider _serviceProvider
        {
            get;
            private set;
        }

        public SearchService(
            IMapper mapper,
            IServiceProvider serviceProvider
        )
        {
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        public virtual async Task<PagedList<TModel>> FindByCriteriaAsync(BaseCriteria<TEntity> criteria)
        {
            DateTime start = DateTime.UtcNow;
            PagedList<TModel> result = new PagedList<TModel>
            {
                Paging = new Paging
                {
                    PageIndex = criteria.PageIndex,
                    PageSize = criteria.PageSize
                }
            };
            result.Paging.BuildingQueryDuration = DateTime.UtcNow.Subtract(start).TotalMilliseconds;

            Task[] tasks = new Task[2]
            {
                CountAsync(criteria, result),
                RetrieveDataAsync(criteria, result)
            };
            await Task.WhenAll(tasks);
            result.Paging.TotalDuration = DateTime.UtcNow.Subtract(start).TotalMilliseconds;
            return result;
        }

        private async Task RetrieveDataAsync(BaseCriteria<TEntity> criteria, PagedList<TModel> result)
        {
            IServiceScopeFactory serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                Type dbType = GetDbType();
                object? context = scope.ServiceProvider.GetService(dbType);
                Ensure.That(context).IsNotNull();
                IQueryable<TEntity> query = BuildFilter(criteria, context);
                DateTime start = DateTime.UtcNow;
                List<TEntity> listResult = await query.Skip(criteria.PageIndex * criteria.PageSize).Take(criteria.PageSize).AsNoTracking()
                    .ToListAsync();
                if (listResult.Any())
                {
                    result.Data = listResult.Select((TEntity x) => _mapper.Map<TModel>(x));
                }
                else
                {
                    result.Data = new List<TModel>();
                }

                result.Paging.QueryDuration = DateTime.UtcNow.Subtract(start).TotalMilliseconds;
            }
        }

        private async Task CountAsync(BaseCriteria<TEntity> criteria, PagedList<TModel> result)
        {
            IServiceScopeFactory serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {
                Type dbType = GetDbType();
                object context = scope.ServiceProvider.GetService(dbType);
                IQueryable<TEntity> query = BuildFilter(criteria, context);
                DateTime start = DateTime.UtcNow;
                Paging paging = result.Paging;
                paging.TotalItemsCount = await query.CountAsync();
                result.Paging.TotalPages = (int)Math.Ceiling((float)result.Paging.TotalItemsCount / (float)criteria.PageSize);
                result.Paging.CountDuration = DateTime.UtcNow.Subtract(start).TotalMilliseconds;
            }
        }

        protected abstract Type GetDbType();

        private IQueryable<TEntity> BuildFilter(BaseCriteria<TEntity> criteria, object context)
        {
            IQueryable<TEntity> queryable = AsQueryable(criteria, context);
            if (criteria.Filter != null)
            {
                foreach (var condition in criteria.Filter)
                {
                    queryable = queryable.Where(condition);
                }               
            }

            if (criteria.Includes != null && criteria.Includes.Count() > 0) {
                foreach (var table in criteria.Includes) { 
                    queryable = queryable.Include(table);              
                }
            }

            if (criteria.SortExps != null)
            {
                if (criteria.Sorts.Contains("DESC"))
                {
                    queryable = queryable.OrderByDescending(criteria.SortExps);
                }
                else {
                    queryable = queryable.OrderBy(criteria.SortExps);
                }
            }

            if (!string.IsNullOrEmpty(criteria.Sorts) && criteria.SortExps == null)
            {
                IEnumerable<string> values = Enumerable.Select(criteria.Sorts.Split(';'), (string x) => x.Replace("=", " "));
                queryable = queryable.OrderBy(string.Join(", ", values));
            }

            if (criteria.Fields != null && criteria.Fields.Any())
            {
                queryable = queryable.Select<TEntity>("new (" + string.Join(",", criteria.Fields) + ")", Array.Empty<object>());
            }

            return queryable;
        }

        protected abstract IQueryable<TEntity> AsQueryable(BaseCriteria<TEntity> criteria, object context);
    }

}


