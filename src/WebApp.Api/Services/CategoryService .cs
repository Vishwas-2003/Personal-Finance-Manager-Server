using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Category;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services
{
    public class CategoryService : CRUDBaseService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository) : base(categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            return await _categoryRepository.GetCategories();
        }
    }
}
