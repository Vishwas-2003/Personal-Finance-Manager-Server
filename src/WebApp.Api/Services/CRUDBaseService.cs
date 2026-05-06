using WebApp.Api.Services.Interfaces;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services
{
    public class CRUDBaseService<T> : ICRUDBaseService<T> where T : class
    {
        private readonly ICRUDBaseRepository<T> _crudBaseRepository;
        public CRUDBaseService(ICRUDBaseRepository<T> crudBaseRepository) { 
            _crudBaseRepository = crudBaseRepository;
        }
        public async Task<T> CreateAsync(T entity)
        {
            return await _crudBaseRepository.CreateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _crudBaseRepository.DeleteAsync(id);
        }

        public async Task<T> ReadAsync(int id)
        {
            return await _crudBaseRepository.ReadAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await _crudBaseRepository.UpdateAsync(entity);
        }
    }
}
