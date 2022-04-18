namespace ForumPersistence.Entity.Forum
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CategoryPost> CategoryPosts { get; set; }
    }
}
