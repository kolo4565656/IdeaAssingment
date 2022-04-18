namespace ForumPersistence.Entity.Forum
{
    public class Post
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public string Description { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string? Content { get; set; }
        public int? TotalStar { get; set; }
        public int? CommentCount { get; set; }
        public bool IsAttachs { get; set; }
        public ICollection<CategoryPost> CategoryPosts { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
