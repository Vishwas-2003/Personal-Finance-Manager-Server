using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Api.Controllers;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.User;

namespace UnitTests;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userService = new();

    [Fact]
    public async Task GetUser_returns_Ok_with_profile_when_user_exists()
    {
        var profile = new UserResponseModel
        {
            Id = 1,
            Name = "Jane",
            MobileNumber = "111",
            Age = 28,
            Address = "Addr",
            Email = "jane@example.com",
        };
        _userService.Setup(s => s.GetUserById(1)).ReturnsAsync(profile);
        var sut = new UserController(_userService.Object);

        var result = await sut.GetUser(1);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(profile, ok.Value);
    }

    [Fact]
    public async Task GetUser_returns_BadRequest_when_user_not_found()
    {
        _userService.Setup(s => s.GetUserById(1)).ReturnsAsync((UserResponseModel?)null);
        var sut = new UserController(_userService.Object);

        var result = await sut.GetUser(1);

        Assert.IsType<BadRequestResult>(result);
    }
}
