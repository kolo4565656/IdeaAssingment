using ForumPersistence.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ForumPersistence
{
    public class SeedUserData : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Id = new Guid("51dc6ea3-e3c3-4246-8b27-bedb039b5433"),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                FirstName = "Truong",
                LastName = "Le",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "admin123");

            builder.HasData(user);
        }
    }

    public class SeedRoleData : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new ApplicationRole() { 
                    Id = new Guid("de47728f-06b1-4fa5-9afe-62a78111c301"), 
                    Name = "Admin", 
                    ConcurrencyStamp = "1", 
                    NormalizedName = "ADMIN" 
                },
                new ApplicationRole() { 
                    Id = new Guid("09c9b818-4402-4997-9906-86e0fb3fb0de"), 
                    Name = "Staff", 
                    ConcurrencyStamp = "2", 
                    NormalizedName = "STAFF" 
                }
            );
        }
    }

    public class SeedUserRoleData : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            builder.HasData(
                new IdentityUserRole<Guid>() { 
                    RoleId = new Guid("de47728f-06b1-4fa5-9afe-62a78111c301"), 
                    UserId = new Guid("51dc6ea3-e3c3-4246-8b27-bedb039b5433")
                }
            );
        }
    }
}
