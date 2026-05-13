using AutoMapper;
using WebApp.Common.Models.Category;
using WebApp.Data.Entities;

namespace WebApp.Data.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryModel, Category>()
            .ForPath(
                destination => destination.CategoryType.Name,
                options => options.MapFrom(source => source.CategoryType))
            .ReverseMap();
    }
}

