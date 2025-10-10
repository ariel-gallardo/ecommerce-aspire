
using Impecable.Turnos.Application.DTOs;
using Impecable.Turnos.Application.Interfaces;
using Impecable.Turnos.Application.Services;
using Impecable.Turnos.Domain.Entities;
using Impecable.Turnos.Domain.Interfaces;
using Moq;

namespace Impecable.Turnos.UnitTests.Application
{
    public class ProductServiceTests
    {
        private readonly Mock<ITurnosRepository> _mockRepository;
        private readonly IProductService _productService;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<ITurnosRepository>();
            _productService = new ProductService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingProduct_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product("Test", "Description", 19.99m);
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
        }

        [Fact]
        public async Task CreateAsync_ValidProduct_ShouldSucceed()
        {
            // Arrange
            var createDto = new CreateProductDto("Test", "Description", 19.99m);
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Turno>()))
                .ReturnsAsync((Turno p) => p);

            // Act
            var result = await _productService.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Price, result.Price);
        }
    }
}
