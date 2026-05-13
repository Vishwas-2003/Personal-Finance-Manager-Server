using WebApp.Common.Models.Category;
using WebApp.Data.Entities;

namespace WebApp.Data.Repositories.Interfaces
{
    public interface ICategoryRepository : ICRUDBaseRepository<Category>
    {
        Task<List<CategoryModel>> GetCategories();
    }
}
