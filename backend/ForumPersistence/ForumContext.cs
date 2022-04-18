using ForumPersistence.Entity.Forum;
using ForumPersistence.Entity.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ForumPersistence
{
    public class ForumContext : IdentityDbContext<ApplicationUser, ApplicationRole,Guid>
    {
        public ForumContext(DbContextOptions<ForumContext> options) : base(options) { }

        public DbSet<Category> Category { get; set; }
        public DbSet<CategoryPost> CategoryPost { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<SubComment> SubComment { get; set; }
        public DbSet<Post> Post { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //config
            builder.ApplyConfigurationsFromAssembly(typeof(ForumContext).Assembly);
        }
    }
}