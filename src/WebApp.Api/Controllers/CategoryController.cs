using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Infrastructure;
using WebApp.Api.Services.Interfaces;

namespace WebApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController(ICategoryService categoryService) : ApiControllerBase
{
    [HttpGet("get")]
    public Task<ActionResult> GetCategory() =>
        ExecuteAsync(async () =>
        {
            var result = await categoryService.GetCategories();
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        });
}
