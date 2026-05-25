using WebApp.Data.Persistence;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Data.Repositories
{
    public class CRUDBaseRepository<T> : ICRUDBaseRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        public CRUDBaseRepository(AppDbContext dbContext) { 
            _dbContext = dbContext;
        }
        public async Task<T> CreateAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await ReadAsync(id);
            if (entity == null)
            {
                return false;
            }
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<T> ReadAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
