using ForumApplication.Dtos.Search;

namespace ForumApplication.Interfaces.Common
{
    public interface ISearchService<TEntity, TModel, TKey> where TModel : class
    {
        Task<PagedList<TModel>> FindByCriteriaAsync(BaseCriteria<TEntity> criteria);
    }
}
