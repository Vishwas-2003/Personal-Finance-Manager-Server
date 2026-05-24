using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Infrastructure;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Summary;

namespace WebApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SummaryController(ISummaryService summaryService) : ApiControllerBase
{
    [HttpGet("income/{userId}")]
    public Task<ActionResult> GetTotalIncomeSummary([FromRoute] int userId) =>
        ExecuteAsync(async () =>
        {
            var result = await summaryService.GetTotalIncomeSummary(userId);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        });

    [HttpGet("expense/{userId}")]
    public Task<ActionResult> GetTotalExpenseSummary([FromRoute] int userId) =>
        ExecuteAsync(async () =>
        {
            var result = await summaryService.GetTotalExpenseSummary(userId);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        });

    [HttpGet("balance/{userId}")]
    public Task<ActionResult> GetBalanceSummary([FromRoute] int userId, [FromQuery] BalanceSummaryFilter? filter) =>
        ExecuteAsync(async () =>
        {
            var result = await summaryService.GetBalanceSummary(userId, filter);
            return Ok(result);
        });
}
