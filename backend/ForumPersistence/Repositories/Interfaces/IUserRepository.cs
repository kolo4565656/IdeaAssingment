using ForumPersistence.Entity.User;
using Microsoft.AspNetCore.Identity;

namespace ForumPersistence.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<ApplicationUser> Users { get; }
        IQueryable<ApplicationRole> Roles { get; }
        IQueryable<IdentityUserRole<Guid>> UserRoles { get; }
        Task UpdateAsync(ApplicationUser user);

    }
}
