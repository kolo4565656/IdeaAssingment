using ForumPersistence.Constants;

namespace ForumApplication.Dtos
{
    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRoles? Role { get; set; }
    }
}
