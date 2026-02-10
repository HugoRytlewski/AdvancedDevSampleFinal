using AdvancedDevSample.Api.Tests.Integration;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AdvancedDevSample.Tests.API.Integration
{
    public class ClientControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly InMemoryClientRepository _repo;

        public ClientControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _repo = (InMemoryClientRepository)factory.Services.GetRequiredService<IClientRepository>();
            _repo.Clear();
        }

        [Fact]
        public async Task CreateClient_Should_Return_Created()
        {
            // Arrange
            var request = new CreateClientRequest("John Doe", "john@example.com", "123 Main St");

            // Act
            var response = await _client.PostAsJsonAsync("/api/client", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var client = await response.Content.ReadFromJsonAsync<ClientDto>();
            Assert.NotNull(client);
            Assert.Equal(request.Name, client.Name);
            Assert.NotEqual(Guid.Empty, client.Id);
        }

        [Fact]
        public async Task GetAllClients_Should_Return_List()
        {
            // Arrange
            _repo.Seed(new Client("Jane Doe", "jane@example.com", "456 Elm St"));

            // Act
            var response = await _client.GetAsync("/api/client");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var clients = await response.Content.ReadFromJsonAsync<List<ClientDto>>();
            Assert.NotNull(clients);
            Assert.NotEmpty(clients);
        }

        [Fact]
        public async Task GetClient_Should_Return_Ok_When_Found()
        {
            // Arrange
            var client = new Client("Bob Smith", "bob@example.com", "789 Oak St");
            _repo.Seed(client);

            // Act
            var response = await _client.GetAsync($"/api/client/{client.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<ClientDto>();
            Assert.Equal(client.Name, dto.Name);
        }

        [Fact]
        public async Task UpdateAddress_Should_Return_NoContent()
        {
            // Arrange
            var client = new Client("Alice Jones", "alice@example.com", "Old Address");
            _repo.Seed(client);
            var request = new UpdateClientAddressRequest("New Address");

            // Act
            var response = await _client.PutAsJsonAsync($"/api/client/{client.Id}/address", request);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var updated = _repo.GetById(client.Id);
            Assert.Equal("New Address", updated.Address);
        }

        [Fact]
        public async Task GetClient_Should_Return_NotFound_When_Not_Exists()
        {
            // Act
            var response = await _client.GetAsync($"/api/client/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateClient_Should_Return_BadRequest_When_Invalid()
        {
            // Arrange
            var request = new CreateClientRequest("", "invalid-email", ""); // Invalid data

            // Act
            var response = await _client.PostAsJsonAsync("/api/client", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAddress_Should_Return_NotFound_When_Client_Not_Exists()
        {
            // Arrange
            var request = new UpdateClientAddressRequest("New Address");

            // Act
            var response = await _client.PutAsJsonAsync($"/api/client/{Guid.NewGuid()}/address", request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteClient_Should_Return_NoContent()
        {
            // Arrange
            var client = new Client("To Delete", "delete@example.com", "Address");
            _repo.Seed(client);

            // Act
            var response = await _client.DeleteAsync($"/api/client/{client.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(_repo.GetById(client.Id));
        }

    }
}
