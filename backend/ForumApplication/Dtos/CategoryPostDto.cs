using ForumPersistence.Entity.Forum;

namespace ForumApplication.Dtos
{
    public class CategoryPostDto
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public int CategoryId { get; set; }
    }
}
