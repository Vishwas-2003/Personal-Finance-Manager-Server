using UserManagement.Services.Interfaces;
using WebApp.Data.Repositories.Interfaces;

namespace UserManagement.Services;

public class AuthService(IUserRepository userRepository) : IAuthService
{
    public async Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken = default)
    {
        _ = userRepository;
        _ = email;
        _ = cancellationToken;
        throw new NotImplementedException();
    }
}
