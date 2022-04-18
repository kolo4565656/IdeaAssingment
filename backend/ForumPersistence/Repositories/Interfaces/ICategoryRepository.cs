using ForumPersistence.Entity.Forum;

namespace ForumPersistence.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }
        IQueryable<CategoryPost> CategoryPosts { get; }
        Task CreateAsync(Category category);
        Task CreateRangeAsync(IEnumerable<Category> categories);
        Task RemoveAsync(IEnumerable<int> categoryIds);
        Task CreateRangeAsync(IEnumerable<CategoryPost> categories);
        Task RemoveAsync(IEnumerable<CategoryPost> categories);
    }
}
