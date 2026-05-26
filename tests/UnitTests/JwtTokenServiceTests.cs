using Microsoft.Extensions.Options;
using UserManagement.Configuration;
using UserManagement.Services;
using WebApp.Data.Entities;

namespace UnitTests;

public class JwtTokenServiceTests
{
    [Fact]
    public void GenerateTokens_should_return_access_and_refresh_tokens()
    {
        var options = Options.Create(new JwtOptions
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            Secret = "this-is-a-very-long-secret-key-for-testing-jwt",
            AccessTokenMinutes = 30,
            RefreshTokenDays = 7,
        });
        var user = new User
        {
            Id = 42,
            Email = "user@example.com",
            Name = "Test",
            MobileNumber = "9999999999",
            Age = 25,
            Address = "Addr",
            PasswordHash = "hash",
        };

        var sut = new JwtTokenService(options);

        var result = sut.GenerateTokens(user);

        Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));
        Assert.False(string.IsNullOrWhiteSpace(result.RefreshToken));
        Assert.True(result.AccessTokenExpiresAtUtc > DateTime.UtcNow);
    }
}
