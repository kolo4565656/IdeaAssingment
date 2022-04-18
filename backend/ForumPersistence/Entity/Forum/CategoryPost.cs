namespace ForumPersistence.Entity.Forum
{
    public class CategoryPost
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
