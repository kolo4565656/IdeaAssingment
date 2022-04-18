using ForumPersistence.Entity.Forum;
using System.Linq.Expressions;

namespace ForumApplication.Dtos.Search.Criterias
{
    public class CategoryCriteria: BaseCriteria<Category>
    {
        public CategoryCriteria()
        {
            Filter = new List<Expression<Func<Category, bool>>>();
            Includes = new List<string>();
        }
    }
}
