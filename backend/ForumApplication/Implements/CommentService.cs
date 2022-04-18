using AutoMapper;
using EnsureThat;
using ForumApplication.Dtos;
using ForumApplication.Dtos.Search;
using ForumApplication.Extensions;
using ForumApplication.Implements.Common;
using ForumApplication.Interfaces;
using ForumPersistence.Constants;
using ForumPersistence.Entity.Forum;
using ForumPersistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumApplication.Implements
{
    public class CommentService : SearchService<Comment, CommentDto, Guid>, ICommentService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public CommentService(
            IPostRepository postRepository,
            IMapper mapper, 
            IServiceProvider serviceProvider,
            IUserRepository userRepository
        ) : base(mapper, serviceProvider)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public int[] CountLikeAndDislikeAsync(Guid postId)
        {
            var result = _postRepository.Comments.Where(x => x.PostId == postId).Select(x => x.Name).ToList();
            if (result.Count == 0) return new int[] { 0, 0 };
            return new int[] { result.Count(x => x == "Like"), result.Count(x => x == "Dislike") };
        }

        public async Task<CommentDto> CreateCommentAsync(CommentCreateDto dto, Guid postId, Guid sub) { 
            Ensure.That(dto).IsNotNull();
            var createEntity = _mapper.Map<Comment>(dto);
            createEntity.UserId = sub;
            createEntity.PostId = postId;
            var entity = await _postRepository.CreateCommentAsync(createEntity);
            await _postRepository.UpdateTotalStarAndCountAsync(dto.Rating, PostAction.Plus, PostAction.Plus, postId);

            var post = await _postRepository.Posts.FirstOrDefaultAsync(x => x.Id == postId);
            var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Id == post.CreatedBy);
            var sender = new EmailSender("Someone has commented on your post!", user.Email);
            sender.Send($"<h1>Rating: {dto.Rating}</h1><p>Content: {dto.Content}</p>");

            return _mapper.Map<CommentDto>(entity);
        }

        public async Task<CommentDto> GetByUserAsync(Guid postId, Guid userId)
        {
            var entity = await _postRepository.Comments.FirstOrDefaultAsync(x => x.UserId == userId && x.PostId == postId);
            if (entity == null) return null;
            return _mapper.Map<CommentDto>(entity);
        }

        public async Task UpdateCommentAsync(CommentUpdateDto dto, Guid commentId, Guid postId)
        {
            Ensure.That(dto).IsNotNull();
            var entity = _mapper.Map<Comment>(dto);
            entity.Id = commentId;
            int commentStar = await _postRepository.Comments.Where(x => x.Id == commentId)
                .Select(x => x.Rating).FirstOrDefaultAsync();
            await _postRepository.UpdateCommentAsync(entity);
            if (dto.Rating != null && dto.Rating != default(int)) {
                if (commentStar != default(int) && commentStar > dto.Rating) {
                    await _postRepository.UpdateTotalStarAsync(commentStar - (int)dto.Rating, PostAction.Minus, postId);
                } 
                else if (commentStar != default(int) && commentStar < dto.Rating)
                {
                    await _postRepository.UpdateTotalStarAsync((int)dto.Rating - commentStar, PostAction.Plus, postId);
                }
            }
        }

        public async Task DeleteCommentAsync(Guid id, Guid postId)
        {
            Ensure.That(id).IsNotEmpty();
            int commentStar = await _postRepository.Comments.Where(x => x.Id == id).Select(x => x.Rating).FirstOrDefaultAsync();           
            await _postRepository.DeleteCommentAsync(id);
            await _postRepository.UpdateTotalStarAndCountAsync(commentStar, PostAction.Minus, PostAction.Minus, postId);
        }

        public override Task<PagedList<CommentDto>> FindByCriteriaAsync(BaseCriteria<Comment> criteria)
        {
            return base.FindByCriteriaAsync(criteria);
        }

        protected override IQueryable<Comment> AsQueryable(BaseCriteria<Comment> criteria, object context)
        {
            return (context as IPostRepository).Comments.AsNoTracking();
        }

        protected override Type GetDbType()
        {
            return typeof(IPostRepository);
        }
    }
}
