using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using UserManagement.Configuration;
using UserManagement.Services;
using UserManagement.Services.Interfaces;
using WebApp.Common.Models.Auth;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace UnitTests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher<User>> _passwordHasher = new();
    private readonly Mock<IJwtTokenService> _jwtTokenService = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly IOptions<JwtOptions> _jwtOptions = Options.Create(new JwtOptions
    {
        RefreshTokenDays = 7,
    });

    [Fact]
    public async Task RegisterAsync_should_create_user_and_return_tokens()
    {
        var request = new RegisterRequest
        {
            Name = "Test User",
            MobileNumber = "9999999999",
            Age = 30,
            Address = "Test Address",
            Email = "test@example.com",
            Password = "Pass@123",
        };
        var tokenResponse = new AuthResponse { AccessToken = "token", RefreshToken = "refresh-token" };

        _userRepository.Setup(x => x.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        _passwordHasher.Setup(x => x.HashPassword(It.IsAny<User>(), request.Password))
            .Returns("hashed-password");
        _jwtTokenService.Setup(x => x.GenerateTokens(It.IsAny<User>()))
            .Returns(tokenResponse);
        _mapper.Setup(x => x.Map<User>(request)).Returns(new User
        {
            Name = "Test User",
            MobileNumber = "9999999999",
            Age = 30,
            Address = "Test Address",
            Email = "test@example.com",
            PasswordHash = string.Empty,
            CreatedAtUtc = DateTime.UtcNow,
        });

        var sut = CreateSut();

        var result = await sut.RegisterAsync(request);

        Assert.Equal(tokenResponse.AccessToken, result.AccessToken);
        Assert.Equal(tokenResponse.RefreshToken, result.RefreshToken);
        _userRepository.Verify(x => x.AddAsync(It.Is<User>(u =>
            u.Email == "test@example.com" &&
            u.PasswordHash == "hashed-password" &&
            u.RefreshToken == tokenResponse.RefreshToken), It.IsAny<CancellationToken>()), Times.Once);
        _userRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_should_throw_when_email_already_exists()
    {
        var request = new RegisterRequest
        {
            Name = "Existing User",
            MobileNumber = "9999999998",
            Age = 31,
            Address = "Existing Address",
            Email = "existing@example.com",
            Password = "Pass@123",
        };
        _userRepository.Setup(x => x.GetByEmailAsync("existing@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User
            {
                Name = "Existing User",
                MobileNumber = "9999999998",
                Age = 31,
                Address = "Existing Address",
                Email = "existing@example.com",
                PasswordHash = "hash",
            });
        _mapper.Setup(x => x.Map<User>(request)).Returns(new User
        {
            Name = "Existing User",
            MobileNumber = "9999999998",
            Age = 31,
            Address = "Existing Address",
            Email = "existing@example.com",
            PasswordHash = string.Empty,
            CreatedAtUtc = DateTime.UtcNow,
        });

        var sut = CreateSut();

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.RegisterAsync(request));
    }

    [Fact]
    public async Task LoginAsync_should_return_tokens_when_credentials_are_valid()
    {
        var request = new LoginRequest { Email = "test@example.com", Password = "Pass@123" };
        var user = new User
        {
            Id = 5,
            Name = "Test User",
            MobileNumber = "9999999999",
            Age = 30,
            Address = "Test Address",
            Email = "test@example.com",
            PasswordHash = "stored-hash",
        };
        var tokenResponse = new AuthResponse { AccessToken = "token", RefreshToken = "new-refresh-token" };

        _userRepository.Setup(x => x.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordHasher.Setup(x => x.VerifyHashedPassword(user, "stored-hash", request.Password))
            .Returns(PasswordVerificationResult.Success);
        _jwtTokenService.Setup(x => x.GenerateTokens(user)).Returns(tokenResponse);

        var sut = CreateSut();

        var result = await sut.LoginAsync(request);

        Assert.Equal("token", result.AccessToken);
        Assert.Equal("new-refresh-token", result.RefreshToken);
        _userRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_should_throw_when_user_not_found()
    {
        var request = new LoginRequest { Email = "missing@example.com", Password = "Pass@123" };
        _userRepository.Setup(x => x.GetByEmailAsync("missing@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var sut = CreateSut();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_should_throw_when_password_is_invalid()
    {
        var request = new LoginRequest { Email = "test@example.com", Password = "wrong" };
        var user = new User
        {
            Name = "Test User",
            MobileNumber = "9999999999",
            Age = 30,
            Address = "Test Address",
            Email = "test@example.com",
            PasswordHash = "stored-hash",
        };

        _userRepository.Setup(x => x.GetByEmailAsync("test@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordHasher.Setup(x => x.VerifyHashedPassword(user, "stored-hash", request.Password))
            .Returns(PasswordVerificationResult.Failed);

        var sut = CreateSut();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.LoginAsync(request));
    }

    [Fact]
    public async Task RefreshAsync_should_throw_when_refresh_token_expiry_is_null()
    {
        var request = new RefreshTokenRequest { RefreshToken = "no-expiry" };
        _userRepository.Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User
            {
                Name = "Test User",
                MobileNumber = "9999999999",
                Age = 30,
                Address = "Test Address",
                Email = "test@example.com",
                PasswordHash = "hash",
                RefreshToken = request.RefreshToken,
                RefreshTokenExpiresUtc = null,
            });

        var sut = CreateSut();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.RefreshAsync(request));
    }

    [Fact]
    public async Task RefreshAsync_should_throw_when_refresh_token_is_unknown()
    {
        var request = new RefreshTokenRequest { RefreshToken = "missing-token" };
        _userRepository.Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var sut = CreateSut();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.RefreshAsync(request));
    }

    [Fact]
    public async Task RefreshAsync_should_throw_when_refresh_token_is_expired()
    {
        var request = new RefreshTokenRequest { RefreshToken = "expired-token" };
        _userRepository.Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User
            {
                Name = "Test User",
                MobileNumber = "9999999999",
                Age = 30,
                Address = "Test Address",
                Email = "test@example.com",
                PasswordHash = "hash",
                RefreshToken = request.RefreshToken,
                RefreshTokenExpiresUtc = DateTime.UtcNow.AddMinutes(-1),
            });

        var sut = CreateSut();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.RefreshAsync(request));
    }

    [Fact]
    public async Task RefreshAsync_should_return_new_tokens_when_refresh_token_is_valid()
    {
        var request = new RefreshTokenRequest { RefreshToken = "valid-token" };
        var user = new User
        {
            Id = 10,
            Name = "Test User",
            MobileNumber = "9999999999",
            Age = 30,
            Address = "Test Address",
            Email = "test@example.com",
            PasswordHash = "hash",
            RefreshToken = "valid-token",
            RefreshTokenExpiresUtc = DateTime.UtcNow.AddDays(1),
        };
        var tokenResponse = new AuthResponse { AccessToken = "new-token", RefreshToken = "rotated-token" };

        _userRepository.Setup(x => x.GetByRefreshTokenAsync(request.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _jwtTokenService.Setup(x => x.GenerateTokens(user)).Returns(tokenResponse);

        var sut = CreateSut();

        var result = await sut.RefreshAsync(request);

        Assert.Equal("new-token", result.AccessToken);
        Assert.Equal("rotated-token", result.RefreshToken);
        _userRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private AuthService CreateSut()
    {
        return new AuthService(
            _userRepository.Object,
            _passwordHasher.Object,
            _jwtTokenService.Object,
            _mapper.Object,
            _jwtOptions);
    }
}

