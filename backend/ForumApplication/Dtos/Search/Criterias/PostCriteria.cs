using ForumPersistence.Entity.Forum;
using System.Linq.Expressions;

namespace ForumApplication.Dtos.Search.Criterias
{
    public class PostCriteria : BaseCriteria<Post>
    {
        public PostCriteria()
        {
            Filter = new List<Expression<Func<Post, bool>>>();
            Includes = new List<string>();
        }
    }
}
