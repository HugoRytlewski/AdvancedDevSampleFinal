using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Services;
using AdvancedDevSample.Domain.ValueObjects;
using AdvancedDevSample.Test.Application.Fakes;
using Xunit;

namespace AdvancedDevSample.Test.Application
{
    public class ProductServiceTests
    {
        private readonly FakeProductRepository _fakeRepo;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _fakeRepo = new FakeProductRepository();
            _service = new ProductService(_fakeRepo);
        }

        [Fact]
        public void GetAllProducts_Should_Return_Empty_When_No_Products()
        {
            // Arrange - repo is empty

            // Act
            var result = _service.GetAllProducts();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAllProducts_Should_Return_ProductDtos()
        {
            // Arrange
            var product = new Product("Test Product", new Price(100m, "EUR"));
            _fakeRepo.Seed(product);

            // Act
            var result = _service.GetAllProducts().ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(product.Id, result[0].Id);
            Assert.Equal("Test Product", result[0].Name);
            Assert.Equal(100m, result[0].Price);
            Assert.Equal("EUR", result[0].Currency);
        }

        [Fact]
        public void CreateProduct_Should_Save_And_Return_Id()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "New Product",
                Price = 50m,
                Currency = "USD"
            };

            // Act
            var id = _service.CreateProduct(request);

            // Assert
            Assert.NotEqual(Guid.Empty, id);
            Assert.Equal(1, _fakeRepo.Count);

            var saved = _fakeRepo.GetById(id);
            Assert.NotNull(saved);
            Assert.Equal("New Product", saved.Name);
            Assert.Equal(50m, saved.Price.Value);
            Assert.Equal("USD", saved.Price.Currency);
        }

        [Fact]
        public void CreateProduct_Should_Throw_When_Price_Is_Invalid()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "Invalid Product",
                Price = -10m,
                Currency = "EUR"
            };

            // Act & Assert
            Assert.Throws<DomainException>(() => _service.CreateProduct(request));
        }

        [Fact]
        public void ChangePrice_Should_Update_Product_Price()
        {
            // Arrange
            var product = new Product("Price Test", new Price(100m, "EUR"));
            _fakeRepo.Seed(product);

            var request = new ChangePriceRequest { NewPrice = 150m };

            // Act
            _service.ChangePrice(product.Id, request);

            // Assert
            var updated = _fakeRepo.GetById(product.Id);
            Assert.Equal(150m, updated.Price.Value);
        }

        [Fact]
        public void ChangePrice_Should_Throw_When_Product_Not_Found()
        {
            // Arrange
            var request = new ChangePriceRequest { NewPrice = 150m };

            // Act & Assert
            Assert.Throws<ApplicationServiceException>(() => _service.ChangePrice(Guid.NewGuid(), request));
        }

        [Fact]
        public void DeleteProduct_Should_Remove_Product()
        {
            // Arrange
            var product = new Product("Delete Test", new Price(100m, "EUR"));
            _fakeRepo.Seed(product);
            Assert.Equal(1, _fakeRepo.Count);

            // Act
            _service.DeleteProduct(product.Id);

            // Assert
            Assert.Equal(0, _fakeRepo.Count);
        }

        [Fact]
        public void GetProductDto_Should_Return_Dto_When_Found()
        {
            // Arrange
            var product = new Product("DTO Test", new Price(200m, "GBP"));
            _fakeRepo.Seed(product);

            // Act
            var dto = _service.GetProductDto(product.Id);

            // Assert
            Assert.Equal(product.Id, dto.Id);
            Assert.Equal("DTO Test", dto.Name);
            Assert.Equal(200m, dto.Price);
            Assert.Equal("GBP", dto.Currency);
        }

        [Fact]
        public void GetProductDto_Should_Throw_When_Not_Found()
        {
            // Arrange - empty repo

            // Act & Assert
            Assert.Throws<ApplicationServiceException>(() => _service.GetProductDto(Guid.NewGuid()));
        }
    }
}
