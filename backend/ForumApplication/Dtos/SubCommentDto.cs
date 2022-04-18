namespace ForumApplication.Dtos
{
    public class SubCommentDto
    {
        public Guid? Id { get; set; }
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserFullName { get; set; }
    }
}
