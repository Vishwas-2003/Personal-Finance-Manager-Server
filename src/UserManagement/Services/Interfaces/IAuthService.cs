namespace UserManagement.Services.Interfaces;

public interface IAuthService
{
    Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken = default);
}

