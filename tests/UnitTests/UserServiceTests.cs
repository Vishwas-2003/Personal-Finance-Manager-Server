using AutoMapper;
using Moq;
using WebApp.Api.Services;
using WebApp.Common.Models.User;
using WebApp.Data.Entities;
using WebApp.Data.Repositories.Interfaces;

namespace UnitTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IMapper> _mapper = new();

    [Fact]
    public async Task GetUserById_should_return_mapped_user_when_found()
    {
        const int userId = 4;
        var entity = new User
        {
            Id = userId,
            Name = "Jane",
            MobileNumber = "111",
            Age = 28,
            Address = "Addr",
            Email = "jane@example.com",
            PasswordHash = "hash",
            CreatedAtUtc = DateTime.UtcNow,
        };
        var expected = new UserResponseModel
        {
            Id = userId,
            Name = "Jane",
            MobileNumber = "111",
            Age = 28,
            Address = "Addr",
            Email = "jane@example.com",
        };
        _userRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(entity);
        _mapper.Setup(m => m.Map<UserResponseModel>(entity)).Returns(expected);

        var sut = new UserService(_userRepository.Object, _mapper.Object);

        var result = await sut.GetUserById(userId);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetUserById_should_return_null_when_user_not_found()
    {
        _userRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        var sut = new UserService(_userRepository.Object, _mapper.Object);

        var result = await sut.GetUserById(99);

        Assert.Null(result);
        _mapper.Verify(m => m.Map<UserResponseModel>(It.IsAny<User>()), Times.Never);
    }
}
