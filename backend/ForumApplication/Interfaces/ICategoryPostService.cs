using ForumApplication.Dtos;
using ForumApplication.Interfaces.Common;
using ForumPersistence.Entity.Forum;

namespace ForumApplication.Interfaces
{
    public interface ICategoryPostService : ISearchService<CategoryPost, PostDto, int>
    {
    }
}
