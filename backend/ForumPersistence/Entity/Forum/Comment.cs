using ForumPersistence.Entity.User;

namespace ForumPersistence.Entity.Forum
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Rating { get; set; }
        public ApplicationUser User { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
    }
}
