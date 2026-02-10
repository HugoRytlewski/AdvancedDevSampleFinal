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
            var request = new CreateFournisseurRequest("MegaCorp", "contact@megacorp.com");

            // Act
            var response = await _client.PostAsJsonAsync("/api/fournisseur", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<FournisseurDto>();
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
        }

        [Fact]
        public async Task GetAll_Should_Return_List()
        {
            // Arrange
            _repo.Seed(new Fournisseur("Supplier A", "a@supp.com"));

            // Act
            var response = await _client.GetAsync("/api/fournisseur");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var list = await response.Content.ReadFromJsonAsync<List<FournisseurDto>>();
            Assert.NotEmpty(list);
        }

         [Fact]
        public async Task GetById_Should_Return_Ok()
        {
            // Arrange
            var f = new Fournisseur("Supplier B", "b@supp.com");
            _repo.Seed(f);

            // Act
            var response = await _client.GetAsync($"/api/fournisseur/{f.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Should_Return_NoContent()
        {
            // Arrange
            var f = new Fournisseur("To Delete", "del@supp.com");
            _repo.Seed(f);

            // Act
            var response = await _client.DeleteAsync($"/api/fournisseur/{f.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Null(_repo.GetById(f.Id));
        }
    }
}
