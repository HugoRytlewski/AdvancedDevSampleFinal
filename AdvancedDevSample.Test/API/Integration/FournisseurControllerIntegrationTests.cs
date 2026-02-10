using AdvancedDevSample.Api.Tests.Integration;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AdvancedDevSample.Tests.API.Integration
{
    public class FournisseurControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly InMemoryFournisseurRepository _repo;

        public FournisseurControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _repo = (InMemoryFournisseurRepository)factory.Services.GetRequiredService<IFournisseurRepository>();
            _repo.Clear();
        }

        [Fact]
        public async Task CreateFournisseur_Should_Return_Created()
        {
            // Arrange
            var request = new CreateFournisseurRequest("Supplier A", "contact@supplier.com");

            // Act
            var response = await _client.PostAsJsonAsync("/api/fournisseur", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<FournisseurDto>();
            Assert.NotNull(dto);
            Assert.Equal(request.Name, dto.Name);
        }

        [Fact]
        public async Task GetFournisseur_Should_Return_Ok_When_Found()
        {
            // Arrange
            var fournisseur = new Fournisseur("Supplier B", "b@supplier.com");
            _repo.Seed(fournisseur);

            // Act
            var response = await _client.GetAsync($"/api/fournisseur/{fournisseur.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<FournisseurDto>();
            Assert.Equal(fournisseur.Name, dto.Name);
        }

        [Fact]
        public async Task GetFournisseur_Should_Return_NotFound_When_Not_Found()
        {
            // Act
            var response = await _client.GetAsync($"/api/fournisseur/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteFournisseur_Should_Return_NoContent()
        {
            // Arrange
            var fournisseur = new Fournisseur("Supplier C", "c@supplier.com");
            _repo.Seed(fournisseur);

            // Act
            var response = await _client.DeleteAsync($"/api/fournisseur/{fournisseur.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(_repo.GetById(fournisseur.Id));
        }

    }
}
