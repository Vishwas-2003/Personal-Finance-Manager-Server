using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Api.Controllers;
using WebApp.Api.Services.Interfaces;
using WebApp.Common.Models.Category;

namespace UnitTests;

public class CategoryControllerTests
{
    private readonly Mock<ICategoryService> _categoryService = new();

    [Fact]
    public async Task GetCategory_returns_Ok_when_categories_exist()
    {
        var categories = new List<CategoryModel> { TestDataHelper.CreateCategory(1, "Food", 1, "Expense") };
        _categoryService.Setup(s => s.GetCategories()).ReturnsAsync(categories);
        var sut = new CategoryController(_categoryService.Object);

        var result = await sut.GetCategory();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(categories, ok.Value);
    }

    [Fact]
    public async Task GetCategory_returns_BadRequest_when_service_returns_null()
    {
        _categoryService.Setup(s => s.GetCategories()).ReturnsAsync((List<CategoryModel>)null!);
        var sut = new CategoryController(_categoryService.Object);

        var result = await sut.GetCategory();

        Assert.IsType<BadRequestResult>(result);
    }
}
