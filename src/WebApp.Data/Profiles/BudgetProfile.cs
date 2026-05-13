using AutoMapper;
using WebApp.Common.Models.Budget;
using WebApp.Data.Entities;

namespace WebApp.Data.Profiles;

public class BudgetProfile : Profile
{
    public BudgetProfile()
    {
        CreateMap<BudgetModel, Budget>().ReverseMap();
        CreateMap<BudgetResponseModel, Budget>().ReverseMap();
    }
}
