using ForumApplication.Dtos;
using ForumApplication.Interfaces.Common;
using ForumPersistence.Entity.Forum;

namespace ForumApplication.Interfaces
{
    public interface IPostService : ISearchService<Post, PostListDto, Guid>
    {
        Task<PostDto> CreateAsync(PostCreateDto postCreateDto, Guid sub);
        Task<PostUpdateDto> UpdateAsync(PostUpdateDto dto, Guid id, Guid sub);
        Task<PostDto?> GetPostById(Guid id);
        Task DeleteAsync(Guid id);
        Task UpSertPostCategoryAsync(IEnumerable<int> categoryIds, Guid postId);
        Task<List<SubCommentDto>> GetSubCommentAsync(Guid commentId, int take = 3);
        Task UpdateSubCommentAsync(SubCommentDto dto, Guid id);
        Task CreateSubCommentAsync(SubCommentDto dto, Guid id);
        Task DeleteSubCommentAsync(Guid id);
        Task<PostDto> GetByIdAsync(Guid id);
        Task<int[]> GenerateStatisticAsync(int year, int? categoryId);

    }
}
