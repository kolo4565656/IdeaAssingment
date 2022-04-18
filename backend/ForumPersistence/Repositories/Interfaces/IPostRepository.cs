using ForumPersistence.Constants;
using ForumPersistence.Entity.Forum;

namespace ForumPersistence.Repositories.Interfaces
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }
        IQueryable<Comment> Comments { get; }
        IQueryable<SubComment> SubComments { get; }
        Task<Post> CreateAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(Guid id);
        Task<Comment> CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(Guid commentId);
        Task UpdateCommentCountAsync(PostAction action, Guid id);
        Task UpdateTotalStarAsync(int star, PostAction action, Guid id);
        Task UpdateTotalStarAndCountAsync(int star, PostAction commentAction, PostAction starAction, Guid id);
        Task UpdateSubCommentAsync(SubComment subComment);
        Task DeleteSubCommentAsync(Guid subCommentId);
        Task<SubComment> CreateSubCommentAsync(SubComment subComment);

    }
}
