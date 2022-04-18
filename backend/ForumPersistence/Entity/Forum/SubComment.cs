using ForumPersistence.Entity.User;

namespace ForumPersistence.Entity.Forum
{
    public class SubComment
    {
        public Guid Id { get; set; }
        public Guid? CommentId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Comment? Comment { get; set; }
        public ApplicationUser User { get; set; }
    }
}
