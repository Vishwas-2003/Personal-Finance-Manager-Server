using AutoMapper;
using WebApp.Common.Models.Auth;
using WebApp.Common.Models.User;
using WebApp.Data.Entities;

namespace WebApp.Data.Profiles;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterRequest, User>()
            .ForMember(
                destination => destination.Name,
                options => options.MapFrom(source => source.Name.Trim()))
            .ForMember(
                destination => destination.MobileNumber,
                options => options.MapFrom(source => source.MobileNumber.Trim()))
            .ForMember(
                destination => destination.Age,
                options => options.MapFrom(source => source.Age))
            .ForMember(
                destination => destination.Address,
                options => options.MapFrom(source => source.Address.Trim()))
            .ForMember(
                destination => destination.Email,
                options => options.MapFrom(source => source.Email.Trim().ToLowerInvariant()))
            .ForMember(
                destination => destination.CreatedAtUtc,
                options => options.MapFrom(_ => DateTime.UtcNow))
            .ForMember(
                destination => destination.PasswordHash,
                options => options.Ignore())
            .ForMember(
                destination => destination.RefreshToken,
                options => options.Ignore())
            .ForMember(
                destination => destination.RefreshTokenExpiresUtc,
                options => options.Ignore());

        CreateMap<UserResponseModel, User>().ReverseMap();
    }
}
