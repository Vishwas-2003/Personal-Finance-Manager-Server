using WebApp.Common.Models.Category;
using WebApp.Data.Entities;

namespace WebApp.Api.Services.Interfaces
{
    public interface ICategoryService : ICRUDBaseService<Category>
    {
        public Task<List<CategoryModel>> GetCategories();
    }
}
