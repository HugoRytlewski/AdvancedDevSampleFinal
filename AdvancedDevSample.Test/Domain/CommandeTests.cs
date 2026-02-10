using Xunit;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.ValueObjects;
using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Linq;

namespace AdvancedDevSample.Test.Domain
{
    public class CommandeTests
    {
        [Fact]
        public void AddItem_ShouldAddItem_WhenProductIsValid()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var quantity = 2;
            var price = new Price(10m, "EUR");

            // Act
            commande.AddItem(productId, quantity, price);

            // Assert
            Assert.Single(commande.Lignes);
            Assert.Equal(productId, commande.Lignes.First().ProductId);
            Assert.Equal(quantity, commande.Lignes.First().Quantity);
            Assert.Equal(price.Value, commande.Lignes.First().UnitPrice.Value);
            Assert.Equal(20m, commande.CalculateTotal());
        }

        [Fact]
        public void AddItem_ShouldThrowException_WhenCommandeIsNotCreated()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            commande.AddItem(Guid.NewGuid(), 1, new Price(10m, "EUR")); // Add item so it can be validated
            commande.Validate(); // Changes status to Validated

            var productId = Guid.NewGuid();
            var quantity = 1;
            var price = new Price(10m, "EUR");

            // Act & Assert
            Assert.Throws<DomainException>(() => commande.AddItem(productId, quantity, price));
        }

         [Fact]
        public void AddItem_ShouldThrowException_WhenItemAlreadyExists()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var price = new Price(10m, "EUR");
            commande.AddItem(productId, 1, price);

            // Act & Assert
            Assert.Throws<DomainException>(() => commande.AddItem(productId, 2, price));
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItem_WhenItemExists()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var price = new Price(10m, "EUR");
            commande.AddItem(productId, 2, price);

            // Act
            commande.RemoveItem(productId);

            // Assert
            Assert.Empty(commande.Lignes);
            Assert.Equal(0m, commande.CalculateTotal());
        }

        [Fact]
        public void RemoveItem_ShouldThrowException_WhenCommandeIsNotCreated()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            var productId = Guid.NewGuid();
            commande.AddItem(productId, 1, new Price(10, "EUR"));
            commande.Validate();

            // Act & Assert
            Assert.Throws<DomainException>(() => commande.RemoveItem(productId));
        }

        [Fact]
        public void RemoveItem_ShouldDoNothing_WhenItemDoesNotExist()
        {
             // Arrange
            var commande = new Commande(Guid.NewGuid());
            var productId = Guid.NewGuid();

            // Act
            commande.RemoveItem(productId);

            // Assert
            Assert.Empty(commande.Lignes);
        }

        [Fact]
        public void Validate_ShouldChangeStatusToValidated_WhenCommandeHasItems()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            commande.AddItem(Guid.NewGuid(), 1, new Price(10m, "EUR"));

            // Act
            commande.Validate();

            // Assert
            Assert.Equal(CommandeStatus.Validated, commande.Status);
        }

        [Fact]
        public void Validate_ShouldThrowException_WhenCommandeHasNoItems()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());

            // Act & Assert
            Assert.Throws<DomainException>(() => commande.Validate());
        }

        [Fact]
        public void Validate_ShouldThrowException_WhenCommandeAlreadyValidated()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            commande.AddItem(Guid.NewGuid(), 1, new Price(10m, "EUR"));
            commande.Validate();

            // Act & Assert
            Assert.Throws<DomainException>(() => commande.Validate());
        }

        [Fact]
        public void Cancel_ShouldChangeStatusToCancelled_WhenCommandeIsCreatedOrValidated()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            
            // Act
            commande.Cancel();

            // Assert
            Assert.Equal(CommandeStatus.Cancelled, commande.Status);
        }

        [Fact]
        public void Cancel_ShouldBeIdempotent_WhenCommandeIsAlreadyCancelled()
        {
            // Arrange
            var commande = new Commande(Guid.NewGuid());
            commande.Cancel();

            // Act
            commande.Cancel();

            // Assert
            Assert.Equal(CommandeStatus.Cancelled, commande.Status);
        }
        
        [Fact]
        public void Constructor_WithParams_ShouldInitializeProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var date = DateTime.Now;
            var status = CommandeStatus.Validated;

            // Act
            var commande = new Commande(id, clientId, date, status);

            // Assert
            Assert.Equal(id, commande.Id);
            Assert.Equal(clientId, commande.ClientId);
            Assert.Equal(date, commande.Date);
            Assert.Equal(status, commande.Status);
            Assert.Empty(commande.Lignes);
            Assert.Equal(0m, commande.CalculateTotal());
        }
    }
}
