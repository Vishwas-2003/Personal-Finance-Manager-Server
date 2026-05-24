using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Infrastructure;
using WebApp.Api.Services.Interfaces;

namespace WebApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserService userService) : ApiControllerBase
{
    [HttpGet("get/{userId}")]
    public Task<ActionResult> GetUser([FromRoute] int userId) =>
        ExecuteAsync(async () =>
        {
            var result = await userService.GetUserById(userId);
            if (result is not null)
            {
                return Ok(result);
            }

            return BadRequest();
        });
}
