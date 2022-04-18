using ForumPersistence.Entity.Forum;
using System.Linq.Expressions;

namespace ForumApplication.Dtos.Search.Criterias
{
    public class CategoryPostCriteria : BaseCriteria<CategoryPost>
    {
        public CategoryPostCriteria()
        {
            Filter = new List<Expression<Func<CategoryPost, bool>>>();
            Includes = new List<string>();
        }
    }
}
