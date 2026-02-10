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
    public class CommandeControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly InMemoryCommandeRepository _commandeRepo;
        private readonly InMemoryClientRepository _clientRepo;
        private readonly InMemoryProductRepository _productRepo;

        public CommandeControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _commandeRepo = (InMemoryCommandeRepository)factory.Services.GetRequiredService<ICommandeRepository>();
            _clientRepo = (InMemoryClientRepository)factory.Services.GetRequiredService<IClientRepository>();
            _productRepo = (InMemoryProductRepository)factory.Services.GetRequiredService<IProductRepository>();
            
            _commandeRepo.Clear();
            _clientRepo.Clear();
            _productRepo.Clear();
        }

        [Fact]
        public async Task CreateCommande_Should_Return_Created_When_Client_Exists()
        {
            // Arrange
            var client = new Client("John", "john@test.com", "Address");
            _clientRepo.Seed(client);
            var request = new CreateCommandeRequest(client.Id);

            // Act
            var response = await _client.PostAsJsonAsync("/api/commande", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<CommandeDto>();
            Assert.NotNull(dto);
            Assert.Equal(client.Id, dto.ClientId);
        }

        [Fact]
        public async Task CreateCommande_Should_Return_BadRequest_When_Client_Not_Exists()
        {
            // Arrange
            var request = new CreateCommandeRequest(Guid.NewGuid());

            // Act
            var response = await _client.PostAsJsonAsync("/api/commande", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetCommande_Should_Return_Ok_When_Found()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            _commandeRepo.Seed(commande);

            // Act
            var response = await _client.GetAsync($"/api/commande/{commande.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<CommandeDto>();
            Assert.Equal(commande.Id, dto.Id);
        }

        [Fact]
        public async Task GetCommande_Should_Return_NotFound_When_Not_Found()
        {
            // Act
            var response = await _client.GetAsync($"/api/commande/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddItem_Should_Return_Ok_When_Valid()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            _commandeRepo.Seed(commande);
            var product = new Product("Test Product", new Price(10, "EUR"));
            _productRepo.Seed(product);
            var request = new AddItemRequest(product.Id, 2);

            // Act
            var response = await _client.PostAsJsonAsync($"/api/commande/{commande.Id}/items", request);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var updated = _commandeRepo.GetById(commande.Id);
            Assert.Single(updated.Lignes);
            Assert.Equal(20, updated.CalculateTotal());
        }

        [Fact]
        public async Task AddItem_Should_Return_NotFound_When_Commande_Not_Found()
        {
            // Arrange
            var request = new AddItemRequest(Guid.NewGuid(), 1);

            // Act
            var response = await _client.PostAsJsonAsync($"/api/commande/{Guid.NewGuid()}/items", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RemoveItem_Should_Return_Ok_When_Valid()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            var product = new Product("Test Product", new Price(10, "EUR"));
            _productRepo.Seed(product);
            commande.AddItem(product.Id, 2, product.Price);
            _commandeRepo.Seed(commande);

            // Act
            var response = await _client.DeleteAsync($"/api/commande/{commande.Id}/items/{product.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var updated = _commandeRepo.GetById(commande.Id);
            Assert.Empty(updated.Lignes);
        }

        [Fact]
        public async Task CancelCommande_Should_Return_Ok_When_Valid()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            _commandeRepo.Seed(commande);

            // Act
            var response = await _client.PostAsync($"/api/commande/{commande.Id}/cancel", null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var updated = _commandeRepo.GetById(commande.Id);
            Assert.Equal(CommandeStatus.Cancelled, updated.Status);
        }
    }
}
