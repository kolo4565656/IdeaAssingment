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
    public class CategoryService : SearchService<Category, CategoryDto, int>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper, 
            IServiceProvider serviceProvider
        ) : base(mapper, serviceProvider)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public Task<bool> CheckCategoryRelationAsync(IEnumerable<int> categoryIds)
        {
            return _categoryRepository.CategoryPosts.AnyAsync(x => categoryIds.Contains(x.CategoryId));
        }

        public async Task<ResponseResult<string>> CreateAsync(IEnumerable<CategoryDto> categoryDtos) {
            try
            {
                int count = categoryDtos.Count();
                var response = new ResponseResult<string>();
                if (count > 0) {
                    var duplicates = categoryDtos.GroupBy(x => x.Name).Any(g => g.Count() > 1);
                    if (duplicates)
                    {
                        response.AddError("Categories has duplicate");
                        return response;
                    }
                    var categories = categoryDtos.Select(x => x.Name);
                    var existedCategories = _categoryRepository.Categories.Where(x => categories.Contains(x.Name)).Select(x => x.Name);
                    categoryDtos = categoryDtos.Where(x => !existedCategories.Contains(x.Name));
                    await _categoryRepository.CreateRangeAsync(_mapper.Map<IEnumerable<Category>>(categoryDtos));
                }

                return response;
            }
            catch (Exception ex) {
                throw new Exception("Something wrong when adding category", ex);
            }
        }

        public async Task RemoveAsync(IEnumerable<int> categoryIds)
        {
            try
            {
                var count = categoryIds.Count();
                if (count > 0)
                {
                    var existedCategoryCount = _categoryRepository.Categories
                        .Where(x => categoryIds.Contains(x.Id)).Select(x => x.Id).AsEnumerable();
                    await _categoryRepository.RemoveAsync(existedCategoryCount);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Something wrong when deleting category", ex);
            }
        }

        public async Task<CategoryDto?> GetCategoryById(int id)
        {
            Ensure.That(id).IsNotDefault();

            Category category = await _categoryRepository.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null) return null;

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public override Task<PagedList<CategoryDto>> FindByCriteriaAsync(BaseCriteria<Category> criteria)
        {
            return base.FindByCriteriaAsync(criteria);
        }

        protected override IQueryable<Category> AsQueryable(BaseCriteria<Category> criteria, object context)
        {
            return (context as ICategoryRepository).Categories.AsNoTracking();
        }

        protected override Type GetDbType()
        {
            return typeof(ICategoryRepository);
        }
    }
}
