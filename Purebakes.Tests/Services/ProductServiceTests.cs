namespace Purebakes.Tests.Services;

using System.Linq.Expressions;
using PureBakes.Data.Repository.Interface;
using PureBakes.Models;
using PureBakes.Service.Services;
using PureBakes.Service.Services.Interface;
using Purebakes.Tests.Fixtures;

public class ProductServiceTests
{
    private IFixture fixture = new Fixture()
        .Customize(new AutoMoqCustomization())
        .ConfigureToSuppressCircularReferences();

    [Fact]
    public void ProductService_GetAll_ReturnsAllProducts()
    {
        // Arrange
        var products = fixture.CreateMany<Product>().ToList();
        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(productRepository => productRepository.GetAll(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<string>()))
            .Returns(products);
        fixture.Inject(productRepositoryMock.Object);

        var sut = fixture.Create<ProductService>();

        // Act
        var result = sut.GetAll();

        // Assert
        Assert.Equal(products, result);
    }

    [Fact]
    public void ProductService_Get_ReturnsProduct()
    {
        // Arrange
        var product = fixture.Create<Product>();
        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(productRepository => productRepository.Get(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .Returns(product);
        fixture.Inject(productRepositoryMock.Object);

        var sut = fixture.Create<ProductService>();

        // Act
        var result = sut.Get(product.Id);

        // Assert
        Assert.Equal(product, result);
    }

    [Fact]
    public void ProductService_Add_CallsRepository()
    {
        // Arrange
        var product = fixture.Create<Product>();
        var productRepositoryMock = new Mock<IProductRepository>();
        fixture.Inject(productRepositoryMock.Object);

        var sut = fixture.Create<ProductService>();

        // Act
        sut.Add(product);

        // Assert
        productRepositoryMock.Verify(productRepository => productRepository.Add(product), Times.Once);
        productRepositoryMock.Verify(productRepository => productRepository.Save(), Times.Once);
    }

    [Fact]
    public void ProductService_Update_CallsRepository()
    {
        // Arrange
        var product = fixture.Create<Product>();
        var productRepositoryMock = new Mock<IProductRepository>();
        fixture.Inject(productRepositoryMock.Object);

        var sut = fixture.Create<ProductService>();

        // Act
        sut.Update(product);

        // Assert
        productRepositoryMock.Verify(productRepository => productRepository.Update(product), Times.Once);
        productRepositoryMock.Verify(productRepository => productRepository.Save(), Times.Once);
    }

    [Fact]
    public void ProductService_Remove_WhenProductNotFound_ReturnsFalse()
    {
        // Arrange
        var productId = fixture.Create<int>();
        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(productRepository => productRepository.Get(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .Returns((Product?)null);
        fixture.Inject(productRepositoryMock.Object);

        var sut = fixture.Create<ProductService>();

        // Act
        var result = sut.Remove(productId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ProductService_Remove_WhenProductHasImage_CallsFileService()
    {
        // Arrange
        var product= fixture.Create<Product>();
        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(productRepository => productRepository.Get(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .Returns(product);
        fixture.Inject(productRepositoryMock.Object);

        var fileServiceMock = new Mock<IFileService>();
        fixture.Inject(fileServiceMock.Object);

        var sut = fixture.Create<ProductService>();

        // Act
        sut.Remove(product.Id);

        // Assert
        fileServiceMock.Verify(fileService => fileService.RemoveOldImageIfExists(product.ImageUrl), Times.Once);
    }

    [Fact]
    public void ProductService_Remove_CallsRemoveFromRepository()
    {
        // Arrange
        var product= fixture.Create<Product>();
        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(productRepository => productRepository.Get(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .Returns(product);
        fixture.Inject(productRepositoryMock.Object);

        var sut = fixture.Create<ProductService>();

        // Act
        var result = sut.Remove(product.Id);

        // Assert
        productRepositoryMock.Verify(productRepository => productRepository.Remove(product), Times.Once);
        productRepositoryMock.Verify(productRepository => productRepository.Save(), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public void ProductService_GetImageUrlOfProduct_WhenProductImageNotFound_ReturnsEmptyString()
    {
        // Arrange
        var product= fixture
            .Build<Product>()
            .Without(product => product.ImageUrl)
            .Create();

        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(productRepository => productRepository.Get(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .Returns(product);
        fixture.Inject(productRepositoryMock.Object);

        var sut = fixture.Create<ProductService>();

        // Act
        var result = sut.GetImageUrlOfProduct(product.Id);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ProductService_GetImageUrlOfProduct_WhenProductImageFound_ReturnsImageUrlString()
    {
        // Arrange
        var product= fixture.Create<Product>();

        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(productRepository => productRepository.Get(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .Returns(product);
        fixture.Inject(productRepositoryMock.Object);

        var sut = fixture.Create<ProductService>();

        // Act
        var result = sut.GetImageUrlOfProduct(product.Id);

        // Assert
        Assert.Equal(product.ImageUrl, result);
    }

    [Fact]
    public void ProductService_GetImageUrlOfProduct_WhenProductNotFound_ReturnsEmptyString()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(productRepository => productRepository.Get(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .Returns((Product?)null);
        fixture.Inject(productRepositoryMock.Object);

        var sut = fixture.Create<ProductService>();

        // Act
        var result = sut.GetImageUrlOfProduct(this.fixture.Create<int>());

        // Assert
        Assert.Empty(result);
    }
}