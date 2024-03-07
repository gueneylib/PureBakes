namespace Purebakes.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PureBakes.Areas.Customer.Controllers;
using PureBakes.Models;
using PureBakes.Service.Services.Interface;

public class HomeControllerTests
{
    private IFixture fixture = new Fixture()
        .Customize(new AutoMoqCustomization());

    [Fact]
    public void HomeController_Index_ReturnsView()
    {
        // Arrange
        var products = fixture.CreateMany<Product>();
        var productServiceMock = new Mock<IProductService>();
        productServiceMock
            .Setup(productService => productService.GetAll())
            .Returns(products);

        var sut = new HomeController(
            new Mock<ILogger<HomeController>>().Object,
            new Mock<IShoppingCartService>().Object,
            productServiceMock.Object);

        // Act
        var result = sut.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.Model, products);
        productServiceMock.Verify(productService => productService.GetAll(), Times.Once);
    }

    [Fact]
    public void HomeController_Index_OnError_LogsExceptionAndReturnsErrorView()
    {
        // Arrange
        var exception = new Exception("Test failure");
        var productServiceMock = new Mock<IProductService>();
        productServiceMock
            .Setup(productService => productService.GetAll())
            .Throws(exception);

        var logServiceMock = new Mock<ILogger<HomeController>>();

        var sut = new HomeController(
            logServiceMock.Object,
            new Mock<IShoppingCartService>().Object,
            productServiceMock.Object);

        // Act
        var result = sut.Index();

        // Assert
        logServiceMock.Verify(logger => logger.LogError(exception, exception.Message), Times.Once);
        var viewResult = Assert.IsType<RedirectToActionResult>(result);

    }
}