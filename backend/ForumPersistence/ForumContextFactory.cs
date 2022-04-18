using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ForumPersistence
{
    public class ForumContextFactory : IDesignTimeDbContextFactory<ForumContext>
    {
        public ForumContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ForumContext>();
            builder.UseSqlServer(
                "server=(LocalDb)\\MSSQLLocalDB;database=ForumDb;trusted_connection=true");

            return new ForumContext(builder.Options);
        }
    }
}
