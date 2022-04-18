using ForumPersistence.Entity.Forum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumPersistence.EntityConfigurations
{
    public class CategoryPostConfig : IEntityTypeConfiguration<CategoryPost>
    {
        public void Configure(EntityTypeBuilder<CategoryPost> builder)
        {
            builder.ToTable("CategoryPost");
            builder.HasKey(sc => new { sc.CategoryId, sc.PostId });

            builder
                .HasOne(sc => sc.Post)
                .WithMany(s => s.CategoryPosts)
                .HasForeignKey(sc => sc.PostId);


            builder
                .HasOne(sc => sc.Category)
                .WithMany(s => s.CategoryPosts)
                .HasForeignKey(sc => sc.CategoryId);
        }
    }
}

