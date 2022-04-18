using AutoMapper;
using ForumApplication.Dtos;
using ForumPersistence.Entity.Forum;
using ForumPersistence.Entity.User;

namespace ForumApplication.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            MapToPresentation();
            MapToEntity();
        }

        private void MapToEntity()
        {
            CreateMap<UserRegisterDto, ApplicationUser>(MemberList.None);

            CreateMap<PostCreateDto, Post>(MemberList.None)
                .ForMember(
                    entity => entity.CategoryPosts,
                    map => map.MapFrom(dto => dto.CategoryIds != null ? dto.CategoryIds.Select(x => new CategoryPost
                    {
                        CategoryId = x,
                    }) : null)
                )
                .BeforeMap((dto, entity) => {
                    entity.Created = DateTime.UtcNow;
                    entity.LastModified = DateTime.UtcNow;
                });

            CreateMap<PostDto, Post>(MemberList.None)
                .ForMember(
                    entity => entity.CategoryPosts,
                    map => map.MapFrom(dto => dto.Categories != null ? dto.Categories.Select(x => new CategoryPost
                    {
                        CategoryId = x.Id,
                    }) : null)
                );
                
            CreateMap<CategoryDto, Category>(MemberList.None);

            CreateMap<PostUpdateDto, Post?>(MemberList.None)
                .BeforeMap((dto, entity) => entity.LastModified = DateTime.UtcNow);

            CreateMap<CommentCreateDto, Comment>(MemberList.None)
                .BeforeMap((dto, entity) => {
                    entity.UpdatedDate = DateTime.UtcNow;
                    entity.CreatedDate = DateTime.UtcNow;
                });

            CreateMap<CommentUpdateDto, Comment>(MemberList.None)
                .BeforeMap((dto, entity) => {
                    entity.UpdatedDate = DateTime.UtcNow;
                });

            CreateMap<UserUpdateDto, ApplicationUser>(MemberList.None);

            CreateMap<SubCommentDto, SubComment>(MemberList.None)
                .ForMember(x => x.UpdatedDate, map => map.MapFrom(x => DateTime.UtcNow));
        }

        private void MapToPresentation()
        {
            CreateMap<ApplicationUser, UserDto>(MemberList.None);

            CreateMap<Post, PostDto>(MemberList.None)
                .ForMember(
                    dto => dto.Categories,
                    map => map.MapFrom(entity => entity.CategoryPosts != null ? entity.CategoryPosts.Select(x => new CategoryDto {
                        Id = x.CategoryId,
                        Name = x.Category != null ? x.Category.Name : null
                    }) : null)
                );

            CreateMap<Post, PostListDto>(MemberList.None)
                .ForMember(
                    dto => dto.Categories,
                    map => map.MapFrom(entity => entity.CategoryPosts != null ? entity.CategoryPosts.Select(x => new CategoryDto
                    {
                        Id = x.CategoryId,
                        Name = x.Category != null ? x.Category.Name : null
                    }) : null)
                );

            CreateMap<Category, CategoryDto>(MemberList.None);

            CreateMap<CategoryPost, PostDto>(MemberList.None)
                .BeforeMap((entity, dto) =>
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<MapperProfile>();
                    });
                    var mapper = config.CreateMapper();
                    var dtoDto = mapper.Map<PostDto>(entity.Post);

                    dto.Content = dtoDto.Content;
                    dto.Name = dtoDto.Name;
                    dto.CreatedBy = dtoDto.CreatedBy;
                    dto.ModifiedBy = dtoDto.ModifiedBy;
                    dto.Categories = dtoDto.Categories;
                    dto.Created = dtoDto.Created;
                    dto.LastModified = dtoDto.LastModified;
                    dto.Id = dtoDto.Id;
                });

            CreateMap<SubComment, SubCommentDto>(MemberList.None)
                .ForMember(x => x.UserFullName, map => map.MapFrom(x => x.User.FirstName + " " + x.User.LastName));

            CreateMap<Comment, CommentDto>(MemberList.None)
                .ForMember(x => x.UserFullName, map => map.MapFrom(x => x.User.FirstName + " " + x.User.LastName))
                .ForMember(
                    dto => dto.SubCommentDtos,
                    map => map.Ignore()
                );
        }
    }

    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}
