using FluentAssertions;
using Moq;
using TestProduct.API;
using TestProduct.API.Models;

namespace TestProduct.Tests.Unit;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _service = new ProductService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetById_WhenExists_ReturnsProduct()
    {
        // Arrange
        var expected = new Product { Id = 1, Name = "Test" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expected);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(expected.Name);
    }

    [Fact]
    public async Task GetById_WhenIdInvalid_ThrowsException()
    {
        // Arrange + Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetByIdAsync(0)
        );

        // Repository không bao giờ được gọi
        _mockRepo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsNull()
    {
        // Arrange: mock trả về null
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Create_WithValidProduct_CallsRepository()
    {
        // Arrange
        var product = new Product { Name = "New Product", Price = 99.9m };
        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync(product);

        // Act
        var result = await _service.CreateAsync(product);

        // Assert
        result.Should().BeEquivalentTo(product);
        _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Once);
    }
}
