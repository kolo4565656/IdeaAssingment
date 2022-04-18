using ForumPersistence.Entity.Forum;
using System.Linq.Expressions;

namespace ForumApplication.Dtos.Search.Criterias
{
    public class CommentCriteria : BaseCriteria<Comment>
    {
        public CommentCriteria()
        {
            Filter = new List<Expression<Func<Comment, bool>>>();
            Includes = new List<string>();
        }
    }
}
