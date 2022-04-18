using ForumApi.Model;
using ForumApplication.Dtos;
using ForumApplication.Dtos.Search;
using ForumApplication.Interfaces;
using ForumPersistence.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ForumApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserController(
            IUserService userService,
            RoleManager<ApplicationRole> roleManager
        ) 
        { 
            _userService = userService;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterDto userRegisterDto)
        {
            if (await _userService.RegisterAsync(userRegisterDto)) {
                //why return create
                return Created(nameof(UserController), userRegisterDto);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to create user"});
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync([FromBody] UserUpdateDto userUpdateDto, Guid id)
        {
            await  _userService.UpdateAsync(userUpdateDto, id);
            return Ok();
        }

        [HttpPost]
        [Route("password")]
        public async Task<IActionResult> ChangeAsync([FromBody] ChangePasswordDto dto)
        {
            //man in the middle
            var result = await _userService.ChangePasswordAsync(dto);
            if (result == null)
            {
                return BadRequest("User not found");
            }

            return Ok(result.Succeeded);

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLoginDto)
        {
            var resultToken = await _userService.LoginAsync(userLoginDto);
            if (resultToken != null) {
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(resultToken), expiration = resultToken.ValidTo });
            }

            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _userService.RemoveAsync(id);
            return Ok();
        }

        [HttpPatch("{id}/role")]
        public async Task<IActionResult> UpsertRoles(Guid id, [FromBody] IEnumerable<string> roleNames)
        {
            await _userService.UpSertRoleForUserAsync(roleNames, id);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserAsync([FromRoute] Guid id)
        {
            var result = await _userService.GetUserById(id);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

/*        [Authorize(Policy = "AdminAccess")]*/
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(string sort, string keyword, int pageSize = 20, int pageIndex = 0)
        {
            var request = new UserCriteria();
            request.Sorts = sort;
            request.PageIndex = pageIndex;
            request.PageSize = pageSize;          

            if (!string.IsNullOrEmpty(keyword)) { 
                request.Filter.Add((x) => x.FirstName.Contains(keyword)
                    || x.UserName.Contains(keyword)
                    || x.LastName.Contains(keyword)
                    || x.Email.Contains(keyword)
                    || x.Id.ToString().Contains(keyword));
            }

            var result = await _userService.FindByCriteriaAsync(request);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }
    }
}
