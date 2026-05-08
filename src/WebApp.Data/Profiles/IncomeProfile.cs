using AutoMapper;
using WebApp.Common.Models.Income;
using WebApp.Data.Entities;

namespace WebApp.Data.Profiles;

public class IncomeProfile : Profile
{
    public IncomeProfile()
    {
        CreateMap<IncomeModel, Income>().ReverseMap();
        CreateMap<IncomeResponseModel, Income>().ReverseMap();
    }
}

