using AdvancedDevSample.Api.Tests.Integration;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AdvancedDevSample.Tests.API.Integration
{
    public class ProductAsyncControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly InMemoryProductRepository _repo;

        public ProductAsyncControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _repo = (InMemoryProductRepository)factory.Services.GetRequiredService<IProductRepository>();
            _repo.Clear(); // Ensure clean state for each test class
        }

        #region GET /api/products

        [Fact]
        public async Task GetAll_Should_Return_Ok_With_Empty_List()
        {
            // Arrange - repo is empty
            _repo.Clear();

            // Act
            var response = await _client.GetAsync("/api/products");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            Assert.NotNull(products);
            Assert.Empty(products);
        }

        [Fact]
        public async Task GetAll_Should_Return_Ok_With_Products()
        {
            // Arrange
            _repo.Clear();
            var product1 = new Product("Product 1", new Price(10m, "EUR"));
            var product2 = new Product("Product 2", new Price(20m, "USD"));
            _repo.Seed(product1);
            _repo.Seed(product2);

            // Act
            var response = await _client.GetAsync("/api/products");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            Assert.NotNull(products);
            Assert.Equal(2, products.Count);
        }

        #endregion

        #region POST /api/products

        [Fact]
        public async Task Create_Should_Return_Created_With_Id()
        {
            // Arrange
            _repo.Clear();
            var request = new CreateProductRequest
            {
                Name = "New Product",
                Price = 99.99m,
                Currency = "EUR"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/products", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Headers.Location);
        }

        [Fact]
        public async Task Create_Should_Return_BadRequest_When_Price_Invalid()
        {
            // Arrange
            _repo.Clear();
            var request = new CreateProductRequest
            {
                Name = "Invalid Product",
                Price = -5m,
                Currency = "EUR"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/products", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region GET /api/products/{id}

        [Fact]
        public async Task GetById_Should_Return_Ok_When_Found()
        {
            // Arrange
            _repo.Clear();
            var product = new Product("Test Product", new Price(50m, "GBP"));
            _repo.Seed(product);

            // Act
            var response = await _client.GetAsync($"/api/products/{product.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(dto);
            Assert.Equal(product.Id, dto.Id);
            Assert.Equal("Test Product", dto.Name);
        }

        [Fact]
        public async Task GetById_Should_Return_NotFound_When_Missing()
        {
            // Arrange
            _repo.Clear();

            // Act
            var response = await _client.GetAsync($"/api/products/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region PUT /api/products/{id}/price

        [Fact]
        public async Task ChangePrice_Should_Return_NoContent_And_Update()
        {
            // Arrange
            _repo.Clear();
            var product = new Product("Price Change Test", new Price(100m, "EUR"));
            _repo.Seed(product);

            var request = new ChangePriceRequest { NewPrice = 200m };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{product.Id}/price", request);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var updated = _repo.GetById(product.Id);
            Assert.Equal(200m, updated.Price.Value);
        }

        [Fact]
        public async Task ChangePrice_Should_Return_NotFound_When_Missing()
        {
            // Arrange
            _repo.Clear();
            var request = new ChangePriceRequest { NewPrice = 200m };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{Guid.NewGuid()}/price", request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region DELETE /api/products/{id}

        [Fact]
        public async Task Delete_Should_Return_NoContent_And_Remove()
        {
            // Arrange
            _repo.Clear();
            var product = new Product("Delete Test", new Price(50m, "EUR"));
            _repo.Seed(product);

            // Act
            var response = await _client.DeleteAsync($"/api/products/{product.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(_repo.GetById(product.Id));
        }

        #endregion
    }
}