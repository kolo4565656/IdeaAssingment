using ForumApplication.Dtos;
using ForumApplication.Dtos.Search.Criterias;
using ForumApplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryPostService _categoryPostService;
        private readonly ICategoryService _categoryService;

        public CategoryController(
            ICategoryPostService categoryPostService,
            ICategoryService categoryService
        )
        {
            _categoryPostService = categoryPostService;
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CategoryCreate([FromBody] IEnumerable<CategoryDto> dtos)
        {
            var result = await _categoryService.CreateAsync(dtos.Select(x => new CategoryDto { Id = 0, Name = x.Name.ToLower()}));
            if (result.Succeeded) return Ok();
            return BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> CategoryRemove([FromBody] IEnumerable<int> ids)
        {
            if (await _categoryService.CheckCategoryRelationAsync(ids))
            {
                return BadRequest("These categories have post relations");
            }

            await _categoryService.RemoveAsync(ids);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> CategoryGetById(int id)
        {
            var dto = await _categoryService.GetCategoryById(id);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryGet(string sort, string keyword, int pageSize = 1000, int pageIndex = 0)
        {
            var request = new CategoryCriteria();
            request.Sorts = sort;
            request.PageIndex = pageIndex;
            request.PageSize = pageSize;

            if (!string.IsNullOrEmpty(keyword))
            {
                request.Filter.Add((x) => string.IsNullOrEmpty(x.Name) || x.Name.Contains(keyword));
            }

            return Ok(await _categoryService.FindByCriteriaAsync(request));
        }

        [HttpGet("{categoryId}/posts")]
        public async Task<IActionResult> CategoryPostGet(
            int categoryId, string sort, string keyword, int pageSize = 20, int pageIndex = 0)
        {
            var request = new CategoryPostCriteria();
            request.Sorts = sort;
            request.PageIndex = pageIndex;
            request.PageSize = pageSize;
            request.Includes.Add("Post");
            request.Includes.Add("Category");

            if (!string.IsNullOrEmpty(keyword))
            {
                request.Filter.Add((x) => x.CategoryId == categoryId 
                    && (string.IsNullOrEmpty(x.Post.Name) || x.Post.Name.Contains(keyword)));
            }
            else {
                request.Filter.Add((x) => x.CategoryId == categoryId);
            }

            return Ok(await _categoryPostService.FindByCriteriaAsync(request));
        }
    }
}
