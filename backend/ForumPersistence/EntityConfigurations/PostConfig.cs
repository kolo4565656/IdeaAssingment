using ForumPersistence.Entity.Forum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumPersistence.EntityConfigurations
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");
            builder.HasKey(c => c.Id);
            builder.Property(x => x.Name).HasMaxLength(250);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.CommentCount).HasDefaultValue(0);
            builder.Property(x => x.TotalStar).HasDefaultValue(0);
            builder.Property(x => x.IsAttachs).HasDefaultValue(false);
        }
    }
}
