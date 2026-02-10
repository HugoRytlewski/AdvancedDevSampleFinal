using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.ValueObjects;
using AdvancedDevSample.Test.Application.Fakes;
using Xunit;
using System;
using System.Linq;

namespace AdvancedDevSample.Test.Application
{
    public class CommandeServiceTests
    {
        private readonly FakeCommandeRepository _commandeRepo;
        private readonly FakeClientRepository _clientRepo;
        private readonly FakeProductRepository _productRepo;
        private readonly CommandeService _service;

        public CommandeServiceTests()
        {
            _commandeRepo = new FakeCommandeRepository();
            _clientRepo = new FakeClientRepository();
            _productRepo = new FakeProductRepository();
            _service = new CommandeService(_commandeRepo, _clientRepo, _productRepo);
        }

        [Fact]
        public void CreateCommande_ShouldCreate_WhenClientExists()
        {
            var client = new Client("John", "john@example.com", "Address");
            _clientRepo.Save(client);

            var result = _service.CreateCommande(new CreateCommandeRequest(client.Id));

            Assert.NotNull(result);
            Assert.Equal(client.Id, result.ClientId);
            Assert.Equal("Created", result.Status);
        }

        [Fact]
        public void AddItem_ShouldAddItem_WhenProductExists()
        {
            var client = new Client("John", "john@example.com", "Address");
            _clientRepo.Save(client);
            var commande = new Commande(client.Id);
            _commandeRepo.Save(commande);

            var product = new Product("Product 1", new Price(100, "EUR"));
            _productRepo.Save(product);

            _service.AddItem(commande.Id, new AddItemRequest(product.Id, 2));

            var updatedCommande = _commandeRepo.GetById(commande.Id);
            Assert.Single(updatedCommande!.Lignes);
            Assert.Equal(200, updatedCommande.CalculateTotal());
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItem_WhenItemExists()
        {
            // Arrange
            var client = new Client("John", "john@example.com", "Address");
            _clientRepo.Save(client);
            var commande = new Commande(client.Id);
            _commandeRepo.Save(commande);

            var product = new Product("Product 1", new Price(100, "EUR"));
            _productRepo.Save(product);

            _service.AddItem(commande.Id, new AddItemRequest(product.Id, 2));

            // Act
            _service.RemoveItem(commande.Id, product.Id);

            // Assert
            var updatedCommande = _commandeRepo.GetById(commande.Id);
            Assert.Empty(updatedCommande!.Lignes);
            Assert.Equal(0, updatedCommande.CalculateTotal());
        }

        [Fact]
        public void ValidateCommande_ShouldValidate_WhenCommandeIsValid()
        {
            // Arrange
            var client = new Client("John", "john@example.com", "Address");
            _clientRepo.Save(client);
            var commande = new Commande(client.Id);
            _commandeRepo.Save(commande);

            var product = new Product("Product 1", new Price(100, "EUR"));
            _productRepo.Save(product);

            _service.AddItem(commande.Id, new AddItemRequest(product.Id, 2));

            // Act
            _service.ValidateCommande(commande.Id);

            // Assert
            var updatedCommande = _commandeRepo.GetById(commande.Id);
            Assert.Equal(CommandeStatus.Validated, updatedCommande!.Status);
        }

        [Fact]
        public void CancelCommande_ShouldCancel_WhenCommandeExists()
        {
            // Arrange
            var client = new Client("John", "john@example.com", "Address");
            _clientRepo.Save(client);
            var commande = new Commande(client.Id);
            _commandeRepo.Save(commande);

            // Act
            _service.CancelCommande(commande.Id);

            // Assert
            var updatedCommande = _commandeRepo.GetById(commande.Id);
            Assert.Equal(CommandeStatus.Cancelled, updatedCommande!.Status);
        }
    }
}
