using ForumApplication.Dtos;
using ForumApplication.Interfaces.Common;
using ForumPersistence.Entity.User;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace ForumApplication.Interfaces
{
    public interface IUserService : ISearchService<ApplicationUser, UserDto, Guid>
    {
        Task<bool> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<JwtSecurityToken?> LoginAsync(UserLoginDto userLoginDto);
        Task<UserDto?> GetUserById(Guid? id);
        Task RemoveAsync(Guid userId);
        Task UpSertRoleForUserAsync(IEnumerable<string> roleNames, Guid userId);
        Task CreateRolesAsync(IEnumerable<string> roleNames);
        Task DeleteRolesAsync(IEnumerable<RoleDto> roles);
        Task<bool> CheckRoleRelationAsync(IEnumerable<Guid> roleIds);
        Task<RoleDto> GetRoleById(Guid id);
        Task UpdateAsync(UserUpdateDto dto, Guid userId);
        Task<IEnumerable<RoleDto>> GetRoles(string keyword, int take);
        Task<IdentityResult>? ChangePasswordAsync(ChangePasswordDto dto);
    }
}
