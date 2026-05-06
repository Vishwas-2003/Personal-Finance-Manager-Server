using WebApp.Data.Entities;
using WebApp.Data.Persistence;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Data.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        _ = dbContext;
        _ = email;
        _ = cancellationToken;
        throw new NotImplementedException();
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _ = dbContext;
        _ = user;
        _ = cancellationToken;
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _ = dbContext;
        _ = cancellationToken;
        throw new NotImplementedException();
    }
}
