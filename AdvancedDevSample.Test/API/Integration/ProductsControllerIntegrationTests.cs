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
    public class ProductsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly InMemoryProductRepository _repo;

        public ProductsControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _repo = (InMemoryProductRepository)factory.Services.GetRequiredService<IProductRepository>();
            _repo.Clear();
        }

        [Fact]
        public async Task CreateProduct_Should_Return_Created()
        {
            // Arrange
            var request = new CreateProductRequest { Name = "New Product", Price = 50, Currency = "USD" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/products", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<dynamic>(); // Response is { id = ... }
            Assert.NotNull(dto);
        }

        [Fact]
        public async Task GetProduct_Should_Return_Ok_When_Found()
        {
            // Arrange
            var product = new Product("Existing Product", new Price(20, "EUR"));
            _repo.Seed(product);

            // Act
            var response = await _client.GetAsync($"/api/products/{product.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.Equal(product.Name, dto.Name);
        }

        [Fact]
        public async Task GetProduct_Should_Return_NotFound_When_Not_Found()
        {
             // Act
            var response = await _client.GetAsync($"/api/products/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ChangePrice_Should_Return_NoContent_When_Product_Exists()
        {
            // Arrange
            var product = new Product("Product for Price Change", new Price(100, "EUR"));
            _repo.Seed(product);
            var request = new ChangePriceRequest { NewPrice = 150 };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{product.Id}/price", request);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var updated = _repo.GetById(product.Id);
            Assert.Equal(150, updated.Price.Value);
        }

        [Fact]
        public async Task ChangePrice_Should_Return_NotFound_When_Product_Not_Exists()
        {
            // Arrange
            var request = new ChangePriceRequest { NewPrice = 150 };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/products/{Guid.NewGuid()}/price", request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_Should_Return_NoContent()
        {
            // Arrange
            var product = new Product("Product to Delete", new Price(10, "EUR"));
            _repo.Seed(product);

            // Act
            var response = await _client.DeleteAsync($"/api/products/{product.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(_repo.GetById(product.Id));
        }

    }
}
