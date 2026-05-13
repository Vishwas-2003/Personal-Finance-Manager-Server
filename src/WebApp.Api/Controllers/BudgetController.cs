using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Budget;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BudgetController(IMapper _mapper, IBudgetService _budgetService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult> AddBudget(BudgetModel budget)
        {
            var mappedBudget = _mapper.Map<Budget>(budget);
            var result = await _budgetService.CreateAsync(mappedBudget);
            if (result != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("get/{userId}")]
        public async Task<ActionResult> GetBudget([FromRoute] int userId)
        {
            var result = await _budgetService.GetBudgetByUserId(userId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpDelete("delete/{budgetId}")]
        public async Task<ActionResult> DeleteBudget([FromRoute] int budgetId)
        {
            var result = await _budgetService.DeleteBudgetById(budgetId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Delete Failed!");
        }
    }
}
