using ForumPersistence.Entity.Forum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumPersistence.EntityConfigurations
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.Property(c => c.Id).UseIdentityColumn();
            builder.HasIndex(c => c.Name).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(30);
        }
    }
}
