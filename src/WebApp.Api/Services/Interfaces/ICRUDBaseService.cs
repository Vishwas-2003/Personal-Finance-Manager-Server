namespace WebApp.Api.Services.Interfaces
{
    public interface ICRUDBaseService<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<T> ReadAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
