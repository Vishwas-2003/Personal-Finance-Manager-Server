using AutoMapper;
using WebApp.Common.Models.Expense;
using WebApp.Data.Entities;

namespace WebApp.Data.Profiles;

public class ExpenseProfile : Profile
{
    public ExpenseProfile()
    {
        CreateMap<ExpenseModel, Expense>();
    }
}

