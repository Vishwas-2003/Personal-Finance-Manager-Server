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
    public Task<ActionResult> GetIncome([FromRoute] int userId) =>
        ExecuteAsync(async () =>
        {
            var result = await incomeService.GetIncomeByUserId(userId);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
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
