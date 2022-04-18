using ForumApplication.Dtos;
using ForumApplication.Interfaces.Common;
using ForumPersistence.Entity.Forum;

namespace ForumApplication.Interfaces
{
    public interface ICommentService : ISearchService<Comment, CommentDto, Guid>
    {
        Task<CommentDto> CreateCommentAsync(CommentCreateDto dto, Guid postId, Guid sub);
        Task UpdateCommentAsync(CommentUpdateDto dto, Guid commentId, Guid postId);
        Task DeleteCommentAsync(Guid id, Guid postId);
        int[] CountLikeAndDislikeAsync(Guid postId);
        Task<CommentDto> GetByUserAsync(Guid postId, Guid userId);
    }
}
