namespace ForumApplication.Dtos
{
    public class UserDto : BaseModel<Guid>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set;}
        public string Email { get; set; }
        public IEnumerable<string> Role { get; set; }
    }
}
