using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UserManagement.Configuration;
using UserManagement.Services.Interfaces;
using WebApp.Common.Models.Auth;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace UserManagement.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher<User> passwordHasher,
    IJwtTokenService jwtTokenService,
    IMapper mapper,
    IOptions<JwtOptions> jwtOptionsAccessor) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var existingUser = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var user = mapper.Map<User>(request);
        user.PasswordHash = string.Empty;

        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        var authResponse = jwtTokenService.GenerateTokens(user);
        user.RefreshToken = authResponse.RefreshToken;
        user.RefreshTokenExpiresUtc = DateTime.UtcNow.AddDays(jwtOptionsAccessor.Value.RefreshTokenDays);

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        PopulateUserProfile(authResponse, user);
        return authResponse;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var authResponse = jwtTokenService.GenerateTokens(user);
        user.RefreshToken = authResponse.RefreshToken;
        user.RefreshTokenExpiresUtc = DateTime.UtcNow.AddDays(jwtOptionsAccessor.Value.RefreshTokenDays);
        await userRepository.SaveChangesAsync(cancellationToken);

        PopulateUserProfile(authResponse, user);
        return authResponse;
    }

    public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (user is null || user.RefreshTokenExpiresUtc is null || user.RefreshTokenExpiresUtc <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token is invalid or expired.");
        }

        var authResponse = jwtTokenService.GenerateTokens(user);
        user.RefreshToken = authResponse.RefreshToken;
        user.RefreshTokenExpiresUtc = DateTime.UtcNow.AddDays(jwtOptionsAccessor.Value.RefreshTokenDays);
        await userRepository.SaveChangesAsync(cancellationToken);

        PopulateUserProfile(authResponse, user);
        return authResponse;
    }

    private static void PopulateUserProfile(AuthResponse response, User user)
    {
        response.Name = user.Name;
        response.MobileNumber = user.MobileNumber;
        response.Age = user.Age;
        response.Address = user.Address;
        response.Email = user.Email;
    }
}
