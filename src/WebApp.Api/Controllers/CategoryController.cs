using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Services.Interfaces;

namespace WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController(ICategoryService _categoryService) : ControllerBase
    {
        [HttpGet("get")]
        public async Task<ActionResult> GetCategory()
        {
            var result = await _categoryService.GetCategories();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
