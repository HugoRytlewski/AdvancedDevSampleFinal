using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;
using Xunit;
using System;
using System.Linq;

namespace AdvancedDevSample.Test.Domain.Entities
{
    public class CommandeTests
    {
        [Fact]
        public void Constructor_ShouldInitialize_WhenValid()
        {
            var clientId = Guid.NewGuid();
            var commande = new Commande(clientId);

            Assert.NotEqual(Guid.Empty, commande.Id);
            Assert.Equal(clientId, commande.ClientId);
            Assert.Equal(CommandeStatus.Created, commande.Status);
            Assert.Empty(commande.Lignes);
        }

        [Fact]
        public void AddItem_ShouldAddLine_WhenValid()
        {
            var commande = new Commande(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var price = new Price(10, "EUR");

            commande.AddItem(productId, 2, price);

            Assert.Single(commande.Lignes);
            var line = commande.Lignes.First();
            Assert.Equal(productId, line.ProductId);
            Assert.Equal(2, line.Quantity);
            Assert.Equal(price, line.UnitPrice);
        }

        [Fact]
        public void CalculateTotal_ShouldReturnSumOfLines()
        {
            var commande = new Commande(Guid.NewGuid());
            commande.AddItem(Guid.NewGuid(), 2, new Price(10, "EUR")); // 20
            commande.AddItem(Guid.NewGuid(), 1, new Price(5, "EUR"));  // 5

            Assert.Equal(25, commande.CalculateTotal());
        }

        [Fact]
        public void Validate_ShouldChangeStatus_WhenValid()
        {
            var commande = new Commande(Guid.NewGuid());
            commande.AddItem(Guid.NewGuid(), 1, new Price(10, "EUR"));

            commande.Validate();

            Assert.Equal(CommandeStatus.Validated, commande.Status);
        }

        [Fact]
        public void Validate_ShouldThrow_WhenNoLines()
        {
            var commande = new Commande(Guid.NewGuid());
            Assert.Throws<DomainException>(() => commande.Validate());
        }
    }
}
