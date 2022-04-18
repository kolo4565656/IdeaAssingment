using ForumPersistence.Entity.User;
using System.Linq.Expressions;

namespace ForumApplication.Dtos.Search
{
    public class UserCriteria: BaseCriteria<ApplicationUser>
    {
        public UserCriteria()
        {
            Filter = new List<Expression<Func<ApplicationUser, bool>>>();
            Includes = new List<string>();
        }
    }
}
