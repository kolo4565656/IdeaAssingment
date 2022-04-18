using ForumApplication.Dtos;
using ForumApplication.Dtos.Search.Criterias;
using ForumApplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUserService _userService;

        public RoleController(
            IUserService userService
        )
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] IEnumerable<string> role)
        {
            await _userService.CreateRolesAsync(role);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAsync([FromBody] IEnumerable<RoleDto> dtos)
        {
            if (await _userService.CheckRoleRelationAsync(dtos.Select(x => x.Id)))
            {
                return BadRequest("These roles have user relations");
            }

            await _userService.DeleteRolesAsync(dtos);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _userService.GetRoleById(id));
        }

        [HttpGet]
        public async Task<IActionResult> Get(string keyword = "", int take = 10)
        {
            return Ok(await _userService.GetRoles(keyword, take));
        }
    }
}
