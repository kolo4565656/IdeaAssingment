﻿namespace ForumApplication.Dtos
{
    public class PostListDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Created { get; set; }
        public Guid? CreatedBy { get; set; }
        public string CreatorName { get; set; }
        public int CommentCount { get; set; }
        public string ModifierName { get; set; }
        public string Description { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid? ModifiedBy { get; set; }
        public int TotalStar { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
    }
}
