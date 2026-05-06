using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Expense;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;
        public ExpenseController(IExpenseService expenseService, IMapper mapper)
        {
            _expenseService = expenseService;
            _mapper = mapper;
        }
    
        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult> AddExpense(ExpenseModel expense) {
            var mappedExpense = _mapper.Map<Expense>(expense);
            var result = await _expenseService.CreateAsync(mappedExpense);
            if (result != null)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
