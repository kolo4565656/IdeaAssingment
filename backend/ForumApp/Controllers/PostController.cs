using ForumApplication.Constants;
using ForumApplication.Dtos;
using ForumApplication.Dtos.Search.Criterias;
using ForumApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Security.Claims;

namespace ForumApi.Controllers
{
    [Route("api/post")]
    [ApiController]
/*    [Authorize]*/
    public class PostController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public PostController(
            IFileService fileService,
            IPostService postService,
            IUserService userService,
            ICommentService commentService
        )
        {
            _commentService = commentService;
            _postService = postService;
            _userService = userService;
            _fileService = fileService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Upload([FromForm] PostCreateDto postCreateDto)
        {
            Guid.TryParse(
                User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
                out Guid sub
            );

            if ((await _userService.GetUserById(sub)) == null)
            {
                ResponseResult<object> userNotFoundResult = new ResponseResult<object>();
                userNotFoundResult.AddError(
                    new { Errors = $"Post creater with id {sub} not found" }
                );
                return BadRequest(userNotFoundResult);
            }
            if (postCreateDto.Attachments != null && postCreateDto.Attachments.Any())
            {
                postCreateDto.IsAttachs = true;
            }

            var createdEntity = await _postService.CreateAsync(postCreateDto, sub);

            if (postCreateDto.MediaFiles != null && postCreateDto.MediaFiles.Any())
            {
                var result = await _fileService.UploadMediaFileAsync(postCreateDto.MediaFiles, createdEntity.Id.ToString());
                if (!result.Succeeded)
                {
                    result.Message = "Failed to save media file";
                    return BadRequest(result);
                }
            }

            if (postCreateDto.Attachments != null && postCreateDto.Attachments.Any())
            {
                var result = await _fileService.UploadAttachmentAsync(postCreateDto.Attachments, createdEntity.Id.ToString());
                if (!result.Succeeded)
                {
                    result.Message = "Failed to save attachment";
                    return BadRequest(result);
                }               
            }

            return Created(nameof(PostController), createdEntity);
        }

        [HttpPost("{id}/media")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadMedia(Guid id, [FromForm] IList<IFormFile> files)
        {
            if (files != null && files.Any())
            {
                var result = await _fileService.UploadByTypeAsync(files, id.ToString(), FileConstant.MediaFolder);
                if (!result.Succeeded)
                {
                    result.Message = "Failed to save media file";
                    return BadRequest(result);
                }

                return Ok();
            }

            return BadRequest(new ResponseResult<object> {
                Succeeded = false,
                Message = "Empty file list"
            });
        }

        [HttpPost("{id}/attachment")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAttachment(Guid id, [FromForm] IList<IFormFile> files)
        {
            if (files != null && files.Any())
            {
                var result = await _fileService.UploadByTypeAsync(files, id.ToString(), FileConstant.AttachmentFolder);
                if (!result.Succeeded)
                {
                    result.Message = "Failed to save media file";
                    return BadRequest(result);
                }

                return Ok();
            }

            return BadRequest(new ResponseResult<object>
            {
                Succeeded = false,
                Message = "Empty file list"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, PostUpdateDto dto)
        {
            Guid.TryParse(
                User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
                out Guid sub
            );

            if ((await _userService.GetUserById(sub)) == null)
            {
                ResponseResult<object> userNotFoundResult = new ResponseResult<object>();
                userNotFoundResult.AddError(
                    new { Errors = $"Post modifier with id {sub} not found" }
                );
                return BadRequest(userNotFoundResult);
            }

            await _postService.UpdateAsync(dto, id, sub);
            return Ok();
        }

        [HttpPatch("{id}/category")]
        public async Task<IActionResult> UpsertCategories(Guid id, [FromBody] IEnumerable<int> categoryIds)
        {
            await _postService.UpSertPostCategoryAsync(categoryIds, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _postService.DeleteAsync(id);
            await _fileService.ClearAsync(id.ToString());
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var postDto = await _postService.GetPostById(id);
            if(postDto == null) return Ok();
            var creator = await _userService.GetUserById(postDto.CreatedBy);
            var modifier = await _userService.GetUserById(postDto.ModifiedBy);
            postDto.CreatorName = $"{creator?.FirstName} {creator?.LastName}";
            postDto.ModifierName = $"{modifier?.FirstName} {modifier?.LastName}";

            var counter = _commentService.CountLikeAndDislikeAsync(postDto.Id);
            postDto.LikeCount = counter[0];
            postDto.DislikeCount = counter[1];

            return Ok(postDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(string sort, string keyword, int category = -1, int pageSize = 20, int pageIndex = 0, bool isSelfSearch = false)
        {

            var request = new PostCriteria();
            request.Sorts = sort;
            request.PageIndex = pageIndex;
            request.PageSize = pageSize;
            request.Includes.Add("CategoryPosts.Category");

            if (isSelfSearch)
            {
                Guid.TryParse(
                    User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
                    out Guid sub
                );
                request.Filter.Add((x) =>
                   x.CreatedBy == sub);
            }

            if (category > 0) {
                request.Filter.Add((x) =>
                   x.CategoryPosts.Any(x => x.CategoryId == category));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                request.Filter.Add((x) =>
                    string.IsNullOrEmpty(x.Name)
                    || x.CreatedBy == null
                    || x.Name.Contains(keyword)
                    || x.CreatedBy.ToString().Contains(keyword));
            }

            var paging = await _postService.FindByCriteriaAsync(request);            

            var data = paging.Data.ToList();
            data.ForEach(x =>
            {
                var counter = _commentService.CountLikeAndDislikeAsync(x.Id);
                x.LikeCount = counter[0];
                x.DislikeCount = counter[1];
            });
            paging.Data = data;

            return Ok(paging);
        }

        [HttpGet("{id}/attachments")]
        public async Task<IActionResult> GetAttachment(Guid id)
        {
            return Ok(await _fileService.GetAttachmentAsync(id.ToString()));
        }

        [HttpGet("{id}/medias")]
        public async Task<IActionResult> GetMedia(Guid id)
        {
            return Ok(await _fileService.GetMediaFileAsync(id.ToString()));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile([FromBody] string path)
        {
            await _fileService.DeleteAsync(path);
            return Ok();
        }

        [HttpGet("{postId}/comment")]
        public async Task<IActionResult> GetCommentOfCurrentUser(Guid postId)
        {
            Guid.TryParse(
                User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
                out Guid sub
            );

            var result = await _commentService.GetByUserAsync(postId, sub);
            return Ok(result);
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComment(Guid id, string sort, string keyword, int pageSize = 20, int pageIndex = 0)
        {
            Guid.TryParse(
                User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
                out Guid sub
            );

            var request = new CommentCriteria();
            request.Sorts = sort;
            request.PageIndex = pageIndex;
            request.PageSize = pageSize;
            request.Includes.Add("User");
            request.Filter.Add((x) => x.PostId == id && x.UserId != sub);

            if (!string.IsNullOrEmpty(keyword))
            {
                request.Filter.Add((x) =>
                        string.IsNullOrEmpty(x.Name)
                        || x.Name.Contains(keyword) 
                        || x.UserId.ToString().Contains(keyword));
            }

            var result = await _commentService.FindByCriteriaAsync(request);

            return Ok(result);
        }

        [HttpGet("comments/{id}")]
        public async Task<IActionResult> GetSubComment(Guid id, int take)
        {
            return Ok(await _postService.GetSubCommentAsync(id, take));
        }

        [HttpPost("comments/{id}")]
        public async Task<IActionResult> CreateSubComment(SubCommentDto dto, Guid id)
        {
            await _postService.CreateSubCommentAsync(dto, id);
            return Ok();
        }

        [HttpPut("subcomments/{id}")]
        public async Task<IActionResult> UpdateSubComment(SubCommentDto dto, Guid id)
        {
            await _postService.UpdateSubCommentAsync(dto, id);
            return Ok();
        }

        [HttpDelete("subcomments/{id}")]
        public async Task<IActionResult> DeleteSubComment(Guid id)
        {
            await _postService.DeleteSubCommentAsync(id);
            return Ok();
        }

        [HttpPost("{postId}/comments")]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto dto, Guid postId)
        {
            Guid.TryParse(
                User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
                out Guid sub
            );

            if ((await _userService.GetUserById(sub)) == null)
            {
                ResponseResult<object> userNotFoundResult = new ResponseResult<object>();
                userNotFoundResult.AddError(
                    new { Errors = $"User ID {sub} not found" }
                );
                return BadRequest(userNotFoundResult);
            }

            return Ok(await _commentService.CreateCommentAsync(dto, postId, sub));
        }

        [HttpPut("{id}/comments/{commentId}")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateDto dto, Guid commentId, Guid id)
        {
            await _commentService.UpdateCommentAsync(dto, commentId, id);
            return Ok();
        }

        [HttpDelete("{id}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId, Guid id)
        {
            await _commentService.DeleteCommentAsync(commentId, id);
            return Ok();
        }
        [HttpGet("statistic")]
        public async Task<IActionResult> GeneratePostStatistic(int year, int? category = null)
        {
            var result = await _postService.GenerateStatisticAsync(year, category);
            return Ok(result);
        }
    }
}
