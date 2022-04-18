using ForumPersistence.Entity.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumPersistence.EntityConfigurations
{
    public class RoleConfig : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable("AspNetRoles");
            builder.HasIndex(c => c.Name).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(30);
        }
    }
}
