using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Infrastructure;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Budget;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BudgetController(IMapper mapper, IBudgetService budgetService) : ApiControllerBase
{
    [HttpPost("add")]
    public Task<ActionResult> AddBudget(BudgetModel budget) =>
        ExecuteAsync(async () =>
        {
            var mappedBudget = mapper.Map<Budget>(budget);
            var result = await budgetService.CreateAsync(mappedBudget);
            if (result != null)
            {
                return Ok();
            }

            return BadRequest();
        });

    [HttpGet("get/{userId}")]
    public Task<ActionResult> GetBudget([FromRoute] int userId) =>
        ExecuteAsync(async () =>
        {
            var result = await budgetService.GetBudgetByUserId(userId);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        });

    [HttpDelete("delete/{budgetId}")]
    public Task<ActionResult> DeleteBudget([FromRoute] int budgetId) =>
        ExecuteAsync(async () =>
        {
            var result = await budgetService.DeleteBudgetById(budgetId);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Delete Failed!");
        });
}
