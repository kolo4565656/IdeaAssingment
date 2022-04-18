using ForumPersistence.Entity.Forum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumPersistence.EntityConfigurations
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comment");
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.User).WithMany(c => c.Comments).HasForeignKey(c => c.UserId);
            builder.HasOne(c => c.Post).WithMany(c => c.Comments).HasForeignKey(c => c.PostId);
        }
    }
}
