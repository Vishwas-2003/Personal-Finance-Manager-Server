using WebApp.Common.Models.User;

namespace WebApp.Api.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseModel?> GetUserById(int userId);
}
