using ForumPersistence.Entity.Forum;
using Microsoft.AspNetCore.Identity;
namespace ForumPersistence.Entity.User
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
    }
}
