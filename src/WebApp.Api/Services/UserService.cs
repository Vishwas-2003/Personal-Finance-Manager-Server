using AutoMapper;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.User;
using WebApp.Data.Repositories.Interfaces;

namespace WebApp.Api.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<UserResponseModel?> GetUserById(int userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        return user is null ? null : mapper.Map<UserResponseModel>(user);
    }
}
