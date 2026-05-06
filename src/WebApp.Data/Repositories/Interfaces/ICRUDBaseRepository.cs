namespace WebApp.Data.Repositories.Interfaces
{
    public interface ICRUDBaseRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> ReadAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
