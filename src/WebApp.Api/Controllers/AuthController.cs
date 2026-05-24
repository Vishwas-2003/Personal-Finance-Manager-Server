using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Services.Interfaces;
using WebApp.Api.Infrastructure;
using WebApp.Common.Models.Auth;

namespace WebApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken) =>
        ExecuteResultAsync(async () =>
        {
            var response = await authService.RegisterAsync(request, cancellationToken);
            return Ok(response);
        });

    [AllowAnonymous]
    [HttpPost("login")]
    public Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken) =>
        ExecuteResultAsync(async () =>
        {
            var response = await authService.LoginAsync(request, cancellationToken);
            return Ok(response);
        });

    [AllowAnonymous]
    [HttpPost("refresh")]
    public Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken) =>
        ExecuteResultAsync(
            async () =>
            {
                var response = await authService.RefreshAsync(request, cancellationToken);
                return Ok(response);
            },
            treatUnauthorizedAsSessionExpired: true);
}
