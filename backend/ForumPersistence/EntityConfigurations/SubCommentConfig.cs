using ForumPersistence.Entity.Forum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumPersistence.EntityConfigurations
{
    public class SubCommentConfig : IEntityTypeConfiguration<SubComment>
    {
        public void Configure(EntityTypeBuilder<SubComment> builder)
        {
            builder.ToTable("SubComment");
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.User).WithMany(c => c.SubComments).HasForeignKey(c => c.UserId);
            builder.HasOne(c => c.Comment).WithMany(c => c.SubComments).HasForeignKey(c => c.CommentId);
        }
    }
}
