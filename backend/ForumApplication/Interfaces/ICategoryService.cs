using ForumApplication.Dtos;
using ForumApplication.Interfaces.Common;
using ForumPersistence.Entity.Forum;

namespace ForumApplication.Interfaces
{
    public interface ICategoryService : ISearchService<Category, CategoryDto, int>
    {
        Task<ResponseResult<string>> CreateAsync(IEnumerable<CategoryDto> categoryDtos);
        Task RemoveAsync(IEnumerable<int> categoryIds);
        Task<CategoryDto?> GetCategoryById(int id);
        Task<bool> CheckCategoryRelationAsync(IEnumerable<int> categoryIds);
    }
}
