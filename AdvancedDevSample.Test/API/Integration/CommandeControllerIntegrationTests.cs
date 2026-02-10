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
        private readonly InMemoryCommandeRepository _repoCommande;
        private readonly InMemoryClientRepository _repoClient;
        private readonly InMemoryProductRepositoryAsync _repoProduct;

        public CommandeControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _repoCommande = (InMemoryCommandeRepository)factory.Services.GetRequiredService<ICommandeRepository>();
            _repoClient = (InMemoryClientRepository)factory.Services.GetRequiredService<IClientRepository>();
            _repoProduct = (InMemoryProductRepositoryAsync)factory.Services.GetRequiredService<IProductRepository>();
            
            _repoCommande.Clear();
            _repoClient.Clear();
            _repoProduct.Clear();
        }

        [Fact]
        public async Task CreateCommande_Should_Return_Created()
        {
            // Arrange
            var client = new Client("Test Client", "test@test.com", "Address");
            _repoClient.Seed(client);
            var request = new CreateCommandeRequest(client.Id);

            // Act
            var response = await _client.PostAsJsonAsync("/api/commande", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var commande = await response.Content.ReadFromJsonAsync<CommandeDto>();
            Assert.Equal("Created", commande.Status);
        }

        [Fact]
        public async Task AddItem_Should_Update_Total()
        {
            // Arrange
            var client = new Client("Test Client", "test@test.com", "Address");
            _repoClient.Seed(client);
            var product = new Product("Test Product", new Price(10m, "EUR"));
            _repoProduct.Seed(product);
            
            var commande = new Commande(client.Id);
            _repoCommande.Seed(commande);

            var request = new AddItemRequest(product.Id, 2);

            // Act
            var response = await _client.PostAsJsonAsync($"/api/commande/{commande.Id}/items", request);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var updated = _repoCommande.GetById(commande.Id);
            Assert.Equal(20m, updated.CalculateTotal());
        }

        [Fact]
        public async Task ValidateCommande_Should_Change_Status()
        {
             // Arrange
            var client = new Client("Test Client", "test@test.com", "Address");
            _repoClient.Seed(client);
            var product = new Product("Test Product", new Price(10m, "EUR"));
            _repoProduct.Seed(product);
            
            var commande = new Commande(client.Id);
            commande.AddItem(product.Id, 1, product.Price);
            _repoCommande.Seed(commande);

            // Act
            var response = await _client.PostAsync($"/api/commande/{commande.Id}/validate", null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var updated = _repoCommande.GetById(commande.Id);
            Assert.Equal(CommandeStatus.Validated, updated.Status);
        }
    }
}
