using AutoMapper;
using EnsureThat;
using ForumApplication.Dtos;
using ForumApplication.Dtos.Search;
using ForumApplication.Implements.Common;
using ForumApplication.Interfaces;
using ForumPersistence.Entity.Forum;
using ForumPersistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumApplication.Implements
{
    public class CategoryPostService : SearchService<CategoryPost, PostDto, int>, ICategoryPostService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryPostService(
            ICategoryRepository categoryRepository,
            IMapper mapper, 
            IServiceProvider serviceProvider
        ) : base(mapper, serviceProvider)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public override Task<PagedList<PostDto>> FindByCriteriaAsync(BaseCriteria<CategoryPost> criteria)
        {
            return base.FindByCriteriaAsync(criteria);
        }

        protected override IQueryable<CategoryPost> AsQueryable(BaseCriteria<CategoryPost> criteria, object context)
        {
            return (context as ICategoryRepository).CategoryPosts.AsNoTracking();
        }

        protected override Type GetDbType()
        {
            return typeof(ICategoryRepository);
        }
    }
}
