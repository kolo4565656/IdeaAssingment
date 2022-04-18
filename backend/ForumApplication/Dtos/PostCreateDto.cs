using Microsoft.AspNetCore.Http;

namespace ForumApplication.Dtos
{
    public class PostCreateDto
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public bool IsAttachs { get; set; } = false;
        public ICollection<int> CategoryIds { get; set; }
        public IList<IFormFile> MediaFiles { get; set; }
        public IList<IFormFile> Attachments { get; set; }
    }
}
