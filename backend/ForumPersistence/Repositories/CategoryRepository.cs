using ForumPersistence.Entity.Forum;
using ForumPersistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumPersistence.Repositories
{
    public class CategoryRepository :  ICategoryRepository
    {
        private readonly ForumContext _context;

        public CategoryRepository(ForumContext context)
        {
            _context = context;
        }

        public IQueryable<Category> Categories => _context.Category.AsNoTracking();
        public IQueryable<CategoryPost> CategoryPosts => _context.CategoryPost.AsNoTracking();

        public async Task CreateAsync(Category category)
        {
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<Category> categories) {
            await _context.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(IEnumerable<int> categoryIds)
        {
            _context.RemoveRange(categoryIds.Select(x => new Category { Id = x }));
            await _context.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<CategoryPost> categories)
        {
            await _context.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(IEnumerable<CategoryPost> categories)
        {
            _context.RemoveRange(categories);
            await _context.SaveChangesAsync();
        }
    }
}
