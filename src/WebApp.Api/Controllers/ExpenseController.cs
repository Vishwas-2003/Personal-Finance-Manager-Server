using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Infrastructure;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Expense;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExpenseController(IExpenseService expenseService, IMapper mapper) : ApiControllerBase
{
    [HttpPost("add")]
    public Task<ActionResult> AddExpense(ExpenseModel expense) =>
        ExecuteAsync(async () =>
        {
            var mappedExpense = mapper.Map<Expense>(expense);
            var result = await expenseService.CreateAsync(mappedExpense);
            if (result != null)
            {
                return Ok();
            }

            return BadRequest();
        });

    [HttpGet("get/{userId}")]
    public Task<ActionResult> GetExpenses([FromRoute] int userId, [FromQuery] ExpenseListFilter? filter) =>
        ExecuteAsync(async () =>
        {
            var result = await expenseService.GetExpensesByUserId(userId, filter);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        });

    [HttpPut("update/{expenseId}")]
    public Task<ActionResult> UpdateExpense([FromRoute] int expenseId, ExpenseModel expense) =>
        ExecuteAsync(async () =>
        {
            var result = await expenseService.UpdateExpenseById(expenseId, expense);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Update failed!");
        });

    [HttpDelete("delete/{expenseId}")]
    public Task<ActionResult> DeleteExpense([FromRoute] int expenseId) =>
        ExecuteAsync(async () =>
        {
            var result = await expenseService.DeleteExpenseById(expenseId);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Delete Failed!");
        });
}
