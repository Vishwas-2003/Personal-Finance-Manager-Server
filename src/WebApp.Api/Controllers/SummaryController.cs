using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Summary;

namespace WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SummaryController(ISummaryService _summaryService) : ControllerBase
    {

        [HttpGet("income/{userId}")]
        public async Task<ActionResult> GetTotalIncomeSummary([FromRoute] int userId)
        {
            var result = await _summaryService.GetTotalIncomeSummary(userId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("expense/{userId}")]
        public async Task<ActionResult> GetTotalExpenseSummary([FromRoute] int userId)
        {
            var result = await _summaryService.GetTotalExpenseSummary(userId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("balance/{userId}")]
        public async Task<ActionResult> GetBalanceSummary([FromRoute] int userId, [FromQuery] BalanceSummaryFilter? filter)
        {
            var result = await _summaryService.GetBalanceSummary(userId, filter);
            return Ok(result);
        }
    }
}
