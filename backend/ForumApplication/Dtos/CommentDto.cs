namespace ForumApplication.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserFullName { get; set; }
        public ICollection<SubCommentDto> SubCommentDtos { get; set; }
    }
}
