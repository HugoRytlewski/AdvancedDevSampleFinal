using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Test.Application.Fakes;
using Xunit;
using System.Linq;

namespace AdvancedDevSample.Test.Application
{
    public class ClientServiceTests
    {
        private readonly FakeClientRepository _clientRepository;
        private readonly ClientService _clientService;

        public ClientServiceTests()
        {
            _clientRepository = new FakeClientRepository();
            _clientService = new ClientService(_clientRepository);
        }

        [Fact]
        public void CreateClient_ShouldSaveClient_WhenValidRequest()
        {
            // Arrange
            var request = new CreateClientRequest("Jane Doe", "jane@example.com", "456 Elm St");

            // Act
            var result = _clientService.CreateClient(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            Assert.Single(_clientRepository.GetAll());
        }

        [Fact]
        public void UpdateClientAddress_ShouldUpdate_WhenClientExists()
        {
            // Arrange
            var client = new Client("Jane Doe", "jane@example.com", "456 Elm St");
            _clientRepository.Save(client);
            string newAddress = "789 Pine St";

            // Act
            _clientService.UpdateClientAddress(client.Id, newAddress);

            // Assert
            var updatedClient = _clientRepository.GetById(client.Id);
            Assert.Equal(newAddress, updatedClient.Address);
        }
    }
}
