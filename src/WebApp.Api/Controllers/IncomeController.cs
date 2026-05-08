using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Income;
using WebApp.Data.Entities;

namespace WebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomeController(IMapper _mapper, IIncomeService _incomeService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult> AddIncome(IncomeModel income)
        {
            var mappedIncome = _mapper.Map<Income>(income);
            var result = await _incomeService.CreateAsync(mappedIncome);
            if (result != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("get/{userId}")]
        public async Task<ActionResult> GetIncome([FromRoute] int userId)
        {
            var result = await _incomeService.GetIncomeByUserId(userId);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpDelete("delete/{incomeId}")]
        public async Task<ActionResult> DeleteIncome([FromRoute] int incomeId)
        {
            var result = await _incomeService.DeleteIncomeById(incomeId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Delete Failed!");
        }
    }
}
