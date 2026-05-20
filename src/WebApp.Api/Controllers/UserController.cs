using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Services.Interfaces;

namespace WebApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("get/{userId}")]
    public async Task<ActionResult> GetUser([FromRoute] int userId)
    {
        var result = await userService.GetUserById(userId);
        if (result is not null)
        {
            return Ok(result);
        }

        return BadRequest();
    }
}
