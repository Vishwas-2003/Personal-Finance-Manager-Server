using Moq;
using WebApp.Api.Services;
using WebApp.Common.Models.Category;
using WebApp.Data.Repositories.Interfaces;

namespace UnitTests;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _repository = new();

    [Fact]
    public async Task GetCategories_should_delegate_to_repository()
    {
        var expected = new List<CategoryModel>
        {
            TestDataHelper.CreateCategory(1, "Food", 1, "Expense"),
        };
        _repository.Setup(r => r.GetCategories()).ReturnsAsync(expected);

        var sut = new CategoryService(_repository.Object);

        var result = await sut.GetCategories();

        Assert.Equal(expected, result);
        _repository.Verify(r => r.GetCategories(), Times.Once);
    }
}
