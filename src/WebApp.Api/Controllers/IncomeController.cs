using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Infrastructure;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Income;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IncomeController(IMapper mapper, IIncomeService incomeService) : ApiControllerBase
{
    [HttpPost("add")]
    public Task<ActionResult> AddIncome(IncomeModel income) =>
        ExecuteAsync(async () =>
        {
            var mappedIncome = mapper.Map<Income>(income);
            var result = await incomeService.CreateAsync(mappedIncome);
            if (result != null)
            {
                return Ok();
            }

            return BadRequest();
        });

    [HttpGet("get/{userId}")]
    public Task<ActionResult> GetIncome([FromRoute] int userId, [FromQuery] IncomeListFilter? filter) =>
        ExecuteAsync(async () =>
        {
            var result = await incomeService.GetIncomeByUserId(userId, filter);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        });

    [HttpPut("update/{incomeId}")]
    public Task<ActionResult> UpdateIncome([FromRoute] int incomeId, IncomeModel income) =>
        ExecuteAsync(async () =>
        {
            var result = await incomeService.UpdateIncomeById(incomeId, income);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Update failed!");
        });

    [HttpDelete("delete/{incomeId}")]
    public Task<ActionResult> DeleteIncome([FromRoute] int incomeId) =>
        ExecuteAsync(async () =>
        {
            var result = await incomeService.DeleteIncomeById(incomeId);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Delete Failed!");
        });
}
