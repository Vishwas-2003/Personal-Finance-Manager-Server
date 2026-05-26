using Microsoft.AspNetCore.Mvc;
using Moq;
using UserManagement.Services.Interfaces;
using WebApp.Api.Controllers;
using WebApp.Common.Constants;
using WebApp.Common.Models.Api;
using WebApp.Common.Models.Auth;

namespace UnitTests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authService = new();

    [Fact]
    public async Task Register_returns_Ok_with_auth_response()
    {
        var request = new RegisterRequest
        {
            Name = "Test",
            MobileNumber = "9999999999",
            Age = 30,
            Address = "Addr",
            Email = "test@example.com",
            Password = "Pass@123",
        };
        var response = new AuthResponse { AccessToken = "a", RefreshToken = "r" };
        _authService.Setup(s => s.RegisterAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var sut = new AuthController(_authService.Object);

        var result = await sut.Register(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(response, ok.Value);
    }

    [Fact]
    public async Task Login_returns_Ok_with_auth_response()
    {
        var request = new LoginRequest { Email = "test@example.com", Password = "Pass@123" };
        var response = new AuthResponse { AccessToken = "a", RefreshToken = "r" };
        _authService.Setup(s => s.LoginAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var sut = new AuthController(_authService.Object);

        var result = await sut.Login(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(response, ok.Value);
    }

    [Fact]
    public async Task Refresh_returns_Ok_when_token_is_valid()
    {
        var request = new RefreshTokenRequest { RefreshToken = "valid" };
        var response = new AuthResponse { AccessToken = "new", RefreshToken = "rotated" };
        _authService.Setup(s => s.RefreshAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var sut = new AuthController(_authService.Object);

        var result = await sut.Refresh(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(response, ok.Value);
    }

    [Fact]
    public async Task Refresh_returns_session_expired_when_unauthorized()
    {
        var request = new RefreshTokenRequest { RefreshToken = "expired" };
        _authService.Setup(s => s.RefreshAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UnauthorizedAccessException("expired"));
        var sut = new AuthController(_authService.Object);

        var result = await sut.Refresh(request, CancellationToken.None);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, objectResult.StatusCode);
        var body = Assert.IsType<ApiErrorResponse>(objectResult.Value);
        Assert.Equal(ApiErrorCodes.SessionExpired, body.Code);
    }

    [Fact]
    public async Task Register_returns_bad_request_when_email_already_exists()
    {
        var request = new RegisterRequest
        {
            Name = "Test",
            MobileNumber = "9999999999",
            Age = 30,
            Address = "Addr",
            Email = "dup@example.com",
            Password = "Pass@123",
        };
        _authService.Setup(s => s.RegisterAsync(request, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Email already registered."));
        var sut = new AuthController(_authService.Object);

        var result = await sut.Register(request, CancellationToken.None);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(400, objectResult.StatusCode);
    }
}
