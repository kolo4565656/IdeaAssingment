namespace ForumPersistence.Entity.Forum
{
    public class Attachment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Path { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
