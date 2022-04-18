using ForumApplication.Implements;
using ForumApplication.Interfaces;
using ForumApplication.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ForumApplication
{
    public static class ServiceRegister
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MapperProfile).GetTypeInfo().Assembly);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryPostService, CategoryPostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddHttpContextAccessor();
        }
    }
}