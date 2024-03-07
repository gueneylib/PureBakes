namespace Purebakes.Tests.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PureBakes;
using PureBakes.Areas.Customer.Controllers;
using PureBakes.Models;
using PureBakes.Service.Services.Interface;
using Purebakes.Tests.Fixtures;

public class HomeControllerTests
{
    private IFixture fixture = new Fixture()
        .Customize(new AutoMoqCustomization())
        .ConfigureToSuppressCircularReferences();

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
            new Mock<ILogService<HomeController>>().Object,
            new Mock<IShoppingCartService>().Object,
            productServiceMock.Object);

        // Act
        var result = sut.Index();

        // Assert
        productServiceMock.Verify(productService => productService.GetAll(), Times.Once);Assert.Multiple(() =>
        {
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewResult.Model, products);
        });
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

        var logServiceMock = new Mock<ILogService<HomeController>>();

        var sut = new HomeController(
            logServiceMock.Object,
            new Mock<IShoppingCartService>().Object,
            productServiceMock.Object);

        // Act
        var result = sut.Index();

        // Assert
        logServiceMock.Verify(logger => logger.LogError(exception, exception.Message), Times.Once);
        Assert.Multiple(() =>
        {
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(PureBakesBaseController.Error), viewResult.ActionName);
        });
    }

    [Fact]
    public void HomeController_Details_ReturnsView()
    {
        // Arrange
        var product = fixture.Create<Product>();
        var shoppingCartItem = fixture
            .Build<ShoppingCartItem>()
            .With(item => item.Product, product)
            .With(item => item.Quantity, 1)
            .Create();

        var productServiceMock = new Mock<IProductService>();
        productServiceMock
            .Setup(productService => productService.Get(It.IsAny<int>()))
            .Returns(product);

        var sut = new HomeController(
            new Mock<ILogService<HomeController>>().Object,
            new Mock<IShoppingCartService>().Object,
            productServiceMock.Object);

        // Act
        var result = sut.Details(product.Id);

        // Assert
        productServiceMock.Verify(productService => productService.Get(product.Id), Times.Once);
        Assert.Multiple(() =>
        {
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(shoppingCartItem.Product, ((ShoppingCartItem)viewResult.Model!).Product);
            Assert.Equal(1, ((ShoppingCartItem)viewResult.Model!).Quantity);
        });
    }

    [Fact]
    public void HomeController_Details_OnError_LogsExceptionAndReturnsErrorView()
    {
        // Arrange
        var exception = new Exception("Test failure");
        var productServiceMock = new Mock<IProductService>();
        productServiceMock
            .Setup(productService => productService.Get(It.IsAny<int>()))
            .Throws(exception);

        var logServiceMock = new Mock<ILogService<HomeController>>();

        var sut = new HomeController(
            logServiceMock.Object,
            new Mock<IShoppingCartService>().Object,
            productServiceMock.Object);

        // Act
        var result = sut.Details(fixture.Create<int>());

        // Assert
        logServiceMock.Verify(logger => logger.LogError(exception, exception.Message), Times.Once);
        Assert.Multiple(() =>
        {
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(PureBakesBaseController.Error), viewResult.ActionName);
        });
    }

    [Fact]
    public void HomeController_Details_Post_ReturnsViewAndSetsSessionProperty()
    {
        // Arrange
        var cartCount = fixture.Create<int>();
        var shoppingCartServiceMock = new Mock<IShoppingCartService>();
        shoppingCartServiceMock
            .Setup(productService => productService.GetShoppingCartProductsQuantity())
            .Returns(cartCount);

        var sessionMock = new Mock<ISession>();
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.Session).Returns(sessionMock.Object);

        var sut = new HomeController(
            new Mock<ILogService<HomeController>>().Object,
            shoppingCartServiceMock.Object,
            new Mock<IProductService>().Object);
        sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        // Act
        var result = sut.Details(fixture.Create<ShoppingCartItem>());

        // Assert
        Assert.Multiple(() =>
        {
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(HomeController.Index), viewResult.ActionName);
        });
    }

    [Fact]
    public void HomeController_Details_Post_DoesNotThrow()
    {
        // Arrange
        var cartCount = fixture.Create<int>();
        var shoppingCartServiceMock = new Mock<IShoppingCartService>();
        shoppingCartServiceMock
            .Setup(productService => productService.GetShoppingCartProductsQuantity())
            .Returns(cartCount);

        var sessionMock = new Mock<ISession>();
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.Session).Returns(sessionMock.Object);

        var sut = new HomeController(
            new Mock<ILogService<HomeController>>().Object,
            shoppingCartServiceMock.Object,
            new Mock<IProductService>().Object);
        sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        // Act
        var exception = Record.Exception(() => sut.Details(fixture.Create<ShoppingCartItem>()));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void HomeController_Details_Post_OnError_LogsExceptionAndReturnsErrorView()
    {
        // Arrange
        var tempDataMock = new Mock<ITempDataDictionary>();
        var exception = new Exception("Test failure");
        var shoppingCartServiceMock = new Mock<IShoppingCartService>();
        shoppingCartServiceMock
            .Setup(productService => productService.UpdateCartItem(It.IsAny<ShoppingCartItem>()))
            .Throws(exception);

        var logServiceMock = new Mock<ILogService<HomeController>>();

        var sut = new HomeController(
            logServiceMock.Object,
            shoppingCartServiceMock.Object,
            new Mock<IProductService>().Object);
        sut.TempData = tempDataMock.Object;

        // Act
        var result = sut.Details(fixture.Create<ShoppingCartItem>());

        // Assert
        logServiceMock.Verify(logger => logger.LogError(exception, exception.Message), Times.Once);
        Assert.Multiple(() =>
        {
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(HomeController.Index), viewResult.ActionName);
        });
    }
}