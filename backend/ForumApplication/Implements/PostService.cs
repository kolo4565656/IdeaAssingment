using AutoMapper;
using EnsureThat;
using ForumApplication.Dtos;
using ForumApplication.Dtos.Search;
using ForumApplication.Implements.Common;
using ForumApplication.Interfaces;
using ForumPersistence.Entity.Forum;
using ForumPersistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumApplication.Implements
{
    public class PostService : SearchService<Post, PostListDto, Guid>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PostService(
            IPostRepository postRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper, 
            IServiceProvider serviceProvider
        ) : base(mapper, serviceProvider)
        {
            _categoryRepository = categoryRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PostDto> CreateAsync(PostCreateDto postCreateDto, Guid sub) {
            Ensure.That(postCreateDto).IsNotNull();
            try
            {
                var entity = _mapper.Map<Post>(postCreateDto);
                entity.CreatedBy = sub;
                entity.ModifiedBy = sub;
                return _mapper.Map<PostDto>(await _postRepository.CreateAsync(entity));
            }
            catch (Exception ex) {
                throw new Exception("Something wrong when creating post", ex);
            }
        }

        public async Task<PostDto> GetByIdAsync(Guid id) {
            var result = await _postRepository.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (result != null) {
                return _mapper.Map<PostDto>(result);
            }
            return null;
        }

        public async Task<PostUpdateDto> UpdateAsync(PostUpdateDto dto, Guid id, Guid sub)
        {
            Ensure.That(dto).IsNotNull();
            Ensure.That(id).IsNotEmpty();
            Ensure.That(id).IsNotDefault();
            
            try
            {
                var entity = _mapper.Map<Post>(dto);
                entity.Id = id;
                entity.ModifiedBy = sub;

                await _postRepository.UpdateAsync(entity);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("Something wrong when updating post", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            Ensure.That(id).IsNotDefault();

            try
            {
                await _postRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Something wrong when deleting post", ex);
            }
        }

        public async Task<PostDto?> GetPostById(Guid id)
        {
            Ensure.That(id).IsNotEmpty();

            Post post = await _postRepository.Posts.Include(x => x.CategoryPosts).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (post == null) return null;

            PostDto postDto = _mapper.Map<PostDto>(post);
            return postDto;
        }

        public async Task UpSertPostCategoryAsync(IEnumerable<int> categoryIds, Guid postId) { 
            Ensure.That(categoryIds).IsNotNull();

            var existCategories = await _categoryRepository.CategoryPosts
                .Where(x => x.PostId == postId).ToListAsync();

            var deleteCategories = existCategories.Where(x => !categoryIds.Contains(x.CategoryId)).Select(x => x.CategoryId);
            var insertCategories = categoryIds.Where(x => !existCategories.Select(x => x.CategoryId).Contains(x));

            if (insertCategories.Any()) { 
                await _categoryRepository.CreateRangeAsync(
                    insertCategories.Select(x => new CategoryPost { PostId = postId, CategoryId = x})
                );            
            }

            if (deleteCategories.Any()) { 
                await _categoryRepository.RemoveAsync(
                    deleteCategories.Select(x => new CategoryPost { PostId = postId, CategoryId = x })
                );           
            }
        }

        public async Task<List<SubCommentDto>> GetSubCommentAsync(Guid commentId, int take = 3)
        {
            List<SubComment> subComments = new List<SubComment>();
            if (take == 0)
            {
                subComments = await _postRepository.SubComments.Where(x => x.CommentId == commentId)
                    .Include(x => x.User).ToListAsync();
            }
            else {
                subComments = await _postRepository.SubComments.Where(x => x.CommentId == commentId).Take(take)
                    .Include(x => x.User).ToListAsync();
            }

            return _mapper.Map<List<SubCommentDto>>(subComments);
        }

        public Task UpdateSubCommentAsync(SubCommentDto dto, Guid id)
        {
            var entity = _mapper.Map<SubComment>(dto);
            entity.Id = id;
            return _postRepository.UpdateSubCommentAsync(entity);
        }

        public Task CreateSubCommentAsync(SubCommentDto dto, Guid id)
        {
            var entity = _mapper.Map<SubComment>(dto);
            entity.Id = id;
            return _postRepository.CreateSubCommentAsync(entity);
        }

        public Task DeleteSubCommentAsync(Guid id)
        {
            return _postRepository.DeleteSubCommentAsync(id);
        }

        public override async Task<PagedList<PostListDto>> FindByCriteriaAsync(BaseCriteria<Post> criteria)
        {
            PagedList<PostListDto> searchResult;
            try
            {
                searchResult = await base.FindByCriteriaAsync(criteria);
            }
            catch (Exception ex) {
                if (ex.Message == "Divide by zero error encountered.")
                {
                    criteria.SortExps = (x) => x.CommentCount == 0 ? 0 : ((int)x.TotalStar/(int)x.CommentCount);
                    searchResult = await base.FindByCriteriaAsync(criteria);
                }
                else {
                    throw;
                }

            }
            
            var data = searchResult.Data.ToList();
            data.ForEach(x =>
            {
                var user = _userRepository.Users.FirstOrDefault(y => y.Id == x.CreatedBy);
                x.CreatorName = $"{user.FirstName} {user.LastName}";
                user = _userRepository.Users.FirstOrDefault(y => y.Id == x.ModifiedBy);
                x.ModifierName = $"{user.FirstName} {user.LastName}";
            });
            searchResult.Data = data;
            return searchResult;
        }

        protected override IQueryable<Post> AsQueryable(BaseCriteria<Post> criteria, object context)
        {
            return (context as IPostRepository).Posts.AsNoTracking();
        }

        protected override Type GetDbType()
        {
            return typeof(IPostRepository);
        }

        public async Task<int[]> GenerateStatisticAsync(int year, int? categoryId)
        {
            List<Post> posts;
            var results = new int[12];
            if (categoryId != null)
            {
                posts = await _postRepository.Posts.Where(x => x.Created.Value.Year == year && x.CategoryPosts.Any(x => x.CategoryId == categoryId)).ToListAsync();
            }
            else {
                posts = await _postRepository.Posts.Where(x => x.Created.Value.Year == year).ToListAsync();
            }
            
            for (int i = 0; i < 12; i++) {
                results[i] = posts.Count(x => x.Created.Value.Month == i + 1);
            }

            return results;
        }
    }
}
