using ForumPersistence.Entity.User;
using ForumPersistence.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ForumPersistence.Extensions;

namespace ForumPersistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ForumContext _context;

        public UserRepository(ForumContext context)
        {
            _context = context;
        }

        public IQueryable<ApplicationUser> Users => _context.Users.AsNoTracking();
        public IQueryable<ApplicationRole> Roles => _context.Roles.AsNoTracking();
        public IQueryable<IdentityUserRole<Guid>> UserRoles => _context.UserRoles.AsNoTracking();

        public async Task UpdateAsync(ApplicationUser user)
        {
            _context.Update(user);
            var entry = _context.Entry(user);
            entry.AvoidUpdateNull(user);

            await _context.SaveChangesAsync();
        }
    }
}
