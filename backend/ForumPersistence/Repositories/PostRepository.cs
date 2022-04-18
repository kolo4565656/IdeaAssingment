using ForumPersistence.Constants;
using ForumPersistence.Entity.Forum;
using ForumPersistence.Entity.User;
using ForumPersistence.Extensions;
using ForumPersistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumPersistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ForumContext _context;

        public PostRepository(ForumContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Posts => _context.Post.AsNoTracking();
        public IQueryable<SubComment> SubComments => _context.SubComment.AsNoTracking();
        public IQueryable<Comment> Comments => _context.Comment.AsNoTracking();

        public Task UpdateCommentCountAsync(PostAction action, Guid id)
        {
            string formattedId = id.ToString().ToUpper();
            string sql = "UPDATE [dbo].[Post] SET [CommentCount] = [CommentCount] {0} 1 WHERE Id = '{1}'";
            if (action == PostAction.Plus) {
                sql = string.Format(sql, "+", formattedId);
                return _context.Database.ExecuteSqlRawAsync(sql);
            }
            else if (action == PostAction.Minus)
            {
                sql = string.Format(sql, "-", formattedId);
                return _context.Database.ExecuteSqlRawAsync(sql);
            }
            return Task.CompletedTask;
        }

        public Task UpdateTotalStarAsync(int star ,PostAction action, Guid id)
        {
            string formattedId = id.ToString().ToUpper();
            string sql = "UPDATE [dbo].[Post] SET [TotalStar] = [TotalStar] {0} WHERE Id = '{1}'";
            if (action == PostAction.Plus)
            {
                sql = string.Format(sql, $"+ {star}", formattedId);
                return _context.Database.ExecuteSqlRawAsync(sql);
            }
            else if (action == PostAction.Minus)
            {
                sql = string.Format(sql, $"- {star}", formattedId);
                return _context.Database.ExecuteSqlRawAsync(sql);
            }
            return Task.CompletedTask;
        }

        public Task UpdateTotalStarAndCountAsync(int star, PostAction commentAction, PostAction starAction, Guid id)
        {
            string formattedId = id.ToString().ToUpper();
            string sql = "UPDATE [dbo].[Post] SET {0} WHERE Id = '{1}'";

            string buildUpdate = "";

            if (commentAction == PostAction.Plus)
            {
                buildUpdate = $"[CommentCount] = [CommentCount] + 1,";
            }
            else if (commentAction == PostAction.Minus)
            {
                buildUpdate = $"[CommentCount] = [CommentCount] - 1,";
            }

            if (starAction == PostAction.Plus)
            {
                buildUpdate = $"{buildUpdate} [TotalStar] = [TotalStar] + {star}";
            }
            else if (starAction == PostAction.Minus)
            {
                buildUpdate = $"{buildUpdate} [TotalStar] = [TotalStar] - {star}";
            }

            if (string.IsNullOrEmpty(buildUpdate))
            {
                return Task.CompletedTask;
            }
            else {
                return _context.Database.ExecuteSqlRawAsync(string.Format(sql, buildUpdate, formattedId));
            }
        }

        public async Task<Post> CreateAsync(Post post) { 
            var result = await _context.AddAsync(post);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Update(post);
            var entry = _context.Entry(post);
            entry.AvoidUpdateNull(post);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id) {
            _context.Post.Remove(new Post() { Id = id });
            await _context.SaveChangesAsync();
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            var result = await _context.AddAsync(comment);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public Task UpdateCommentAsync(Comment comment) {
            _context.Update(comment);
            var entry = _context.Entry(comment);
            entry.AvoidUpdateNull(comment, ignoreProperty: new Type[] { typeof(ApplicationUser), typeof(Post)});

            return _context.SaveChangesAsync();
        }

        public Task DeleteCommentAsync(Guid commentId)
        {
            _context.Remove(new Comment { Id = commentId });
            return _context.SaveChangesAsync();
        }

        public Task UpdateSubCommentAsync(SubComment subComment)
        {
            _context.Update(subComment);
            var entry = _context.Entry(subComment);
            entry.AvoidUpdateNull(subComment, ignoreProperty: new Type[] { typeof(Comment)});

            return _context.SaveChangesAsync();
        }

        public async Task<SubComment> CreateSubCommentAsync(SubComment subComment)
        {
            var result = await _context.SubComment.AddAsync(subComment);
            await _context.SaveChangesAsync();
            return subComment;
        }

        public Task DeleteSubCommentAsync(Guid subCommentId)
        {
            _context.Remove(new SubComment { Id = subCommentId });
            return _context.SaveChangesAsync();
        }
    }
}
