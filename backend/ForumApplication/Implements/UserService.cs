using AutoMapper;
using EnsureThat;
using ForumApplication.Dtos;
using ForumApplication.Dtos.Search;
using ForumApplication.Extensions;
using ForumApplication.Implements.Common;
using ForumApplication.Interfaces;
using ForumPersistence.Entity.User;
using ForumPersistence.Extensions;
using ForumPersistence.Repositories.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using PasswordGenerator;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ForumApplication.Implements
{
    public class UserService : SearchService<ApplicationUser, UserDto, Guid>, IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserService(
            UserManager<ApplicationUser> userManager, 
            IMapper mapper, 
            RoleManager<ApplicationRole> roleManager, 
            IConfiguration configuration,
            IUserRepository userRepository,
            IServiceProvider serviceProvider
        ) : base(mapper, serviceProvider)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userRegisterDto);
            user.SecurityStamp = Guid.NewGuid().ToString();

            var passwordGenerator = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: false, passwordLength: 8);
            var randomPassword = passwordGenerator.Next();

            var result = await _userManager.CreateAsync(user, randomPassword);
            if (result.Succeeded) {
                var addRoleResult = await _userManager.AddToRoleAsync(user ,MapEnum.MapEnumRole(userRegisterDto.Role));

                if (addRoleResult.Succeeded) {
                    //send mail
                    var sender = new EmailSender("Your Password", userRegisterDto.Email);
                    sender.Send($"<h1>Greenwich Ideal</h1><p>Use this password for your login: {randomPassword}</p>");
                    return true;
                }
            };

            return false;
        }

        public async Task<IdentityResult>? ChangePasswordAsync(ChangePasswordDto dto)
        {
            var entity = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (entity == null) return null;
            return await _userManager.ChangePasswordAsync(entity, dto.CurrentPassword, dto.NewPassword);
        }

        public async Task<JwtSecurityToken?> LoginAsync(UserLoginDto userLoginDto)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(userLoginDto.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("firstName", user.FirstName),
                    new Claim("lastName", user.LastName),
                    new Claim("email", user.Email),
                    new Claim(ClaimTypes.Role, string.Join( ",", userRoles)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

                var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.UtcNow.AddHours(3),
                        claims: claims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return token;
            }

            return null;
        }

        public async Task UpdateAsync(UserUpdateDto dto, Guid userId)
        { 
            var entity = _mapper.Map<ApplicationUser>(dto);
            var userToUpdate = await _userRepository.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var concurrencyStamp = userToUpdate.ConcurrencyStamp;
            var securityStamp = userToUpdate.SecurityStamp;
            entity.Id = userId;
            entity.ConcurrencyStamp = concurrencyStamp;
            entity.SecurityStamp = securityStamp;
            await _userRepository.UpdateAsync(entity);
        }

        public async Task UpSertRoleForUserAsync(IEnumerable<string> roleNames, Guid userId)
        {
            Ensure.That(roleNames).IsNotNull();

            var existRoles = await _userManager.GetRolesAsync(new ApplicationUser { Id = userId});
            var deleteRoles = existRoles.Where(x => !roleNames.Contains(x));
            var insertRoles = roleNames.Where(x => !existRoles.Contains(x));

            if (deleteRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(new ApplicationUser { Id = userId }, deleteRoles);
            }

            if (insertRoles.Any())
            {
                await _userManager.AddToRolesAsync(new ApplicationUser { Id = userId }, insertRoles);
            }
        }

        public async Task RemoveAsync(Guid userId)
        {
            var entity = await _userManager.FindByIdAsync(userId.ToString());
            if (entity == null) return;
            await _userManager.DeleteAsync(entity);
        }

        public async Task<UserDto?> GetUserById(Guid? id)
        {
            Ensure.That(id).IsNotNull();

            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;

            UserDto userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<RoleDto> GetRoleById(Guid id)
        {
            Ensure.That(id).IsNotDefault();

            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return null;

            return new RoleDto { Id = role.Id, Name = role.Name };
        }

        public async Task<IEnumerable<RoleDto>> GetRoles(string keyword, int take)
        {
            Ensure.That(keyword).IsNotNull();

            IEnumerable<ApplicationRole> roles;

            if (!string.IsNullOrEmpty(keyword))
            {
                roles = await _userRepository.Roles.Where(x => x.Name.Contains(keyword)).Take(take).ToListAsync();
            }
            roles = await _userRepository.Roles.Take(take).ToListAsync();

            return roles.Select(x => new RoleDto {Id = x.Id , Name = x.Name});
        }

        public Task<bool> CheckRoleRelationAsync(IEnumerable<Guid> roleIds) {
            return _userRepository.UserRoles.AnyAsync(x => roleIds.Contains(x.RoleId));
        }

        public async Task CreateRolesAsync(IEnumerable<string> roleNames) {
            Ensure.That(roleNames).IsNotNull();

            var existRoles = await _roleManager.Roles
                .Where(x => roleNames.Contains(x.Name)).Select(x => x.Name).ToListAsync();

            var rolesToAdd = roleNames.Where(x => !existRoles.Contains(x));

            foreach (var role in rolesToAdd) {
                await _roleManager.CreateAsync(new ApplicationRole { Name = role, NormalizedName = role.ToUpper()});
            }
        }

        public async Task DeleteRolesAsync(IEnumerable<RoleDto> roles)
        {
            Ensure.That(roles).IsNotNull();

            var existRoles = await _roleManager.Roles
                .Where(x => roles.Select(r => r.Name).Contains(x.Name)).Select(x => new RoleDto { Id = x.Id, Name = x.Name}).ToListAsync();

            var rolesToDelete = roles.Where(x => existRoles.Select(r => r.Name).Contains(x.Name));

            foreach (var role in rolesToDelete)
            {
                var entity = await _roleManager.FindByNameAsync(role.Name);
                if (entity == null) continue;
                await _roleManager.DeleteAsync(entity);
            }
        }

        public override Task<PagedList<UserDto>> FindByCriteriaAsync(BaseCriteria<ApplicationUser> criteria)
        {
            return base.FindByCriteriaAsync(criteria);
        }

        protected override IQueryable<ApplicationUser> AsQueryable(BaseCriteria<ApplicationUser> criteria, object context)
        {
            return (context as IUserRepository).Users.AsNoTracking();
        }

        protected override Type GetDbType()
        {
            return typeof(IUserRepository);
        }
    }
}
