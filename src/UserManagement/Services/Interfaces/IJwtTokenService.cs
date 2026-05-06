using WebApp.Common.Models.Auth;
using WebApp.Data.Entities;

namespace UserManagement.Services.Interfaces;

public interface IJwtTokenService
{
    AuthResponse GenerateTokens(User user);
}

