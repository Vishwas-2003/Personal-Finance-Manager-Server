using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApp.Common.Models.Budget;
using WebApp.Common.Models.Category;
using WebApp.Data.Entities;
using WebApp.Data.Persistence;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Data.Repositories
{
    public class CategoryRepository : CRUDBaseRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            var categories = await _dbContext.Categories
                .Include(c => c.CategoryType)
                .ToListAsync();

            return _mapper.Map<List<CategoryModel>>(categories);
        }
    }
}
