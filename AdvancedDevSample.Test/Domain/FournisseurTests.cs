using Xunit;
using AdvancedDevSample.Domain.Entities;
using System;

namespace AdvancedDevSample.Test.Domain
{
    public class FournisseurTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            var name = "Test Supplier";
            var email = "test@supplier.com";

            // Act
            var fournisseur = new Fournisseur(name, email);

            // Assert
            Assert.NotEqual(Guid.Empty, fournisseur.Id);
            Assert.Equal(name, fournisseur.Name);
            Assert.Equal(email, fournisseur.ContactEmail);
        }

        [Fact]
        public void Constructor_WithId_ShouldInitializeProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Test Supplier";
            var email = "test@supplier.com";

            // Act
            var fournisseur = new Fournisseur(id, name, email);

            // Assert
            Assert.Equal(id, fournisseur.Id);
            Assert.Equal(name, fournisseur.Name);
            Assert.Equal(email, fournisseur.ContactEmail);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ShouldThrow_WhenNameIsInvalid(string name)
        {
            // Act & Assert
            Assert.Throws<AdvancedDevSample.Domain.Exceptions.DomainException>(() => new Fournisseur(name, "email@test.com"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ShouldThrow_WhenEmailIsInvalid(string email)
        {
             // Act & Assert
            Assert.Throws<AdvancedDevSample.Domain.Exceptions.DomainException>(() => new Fournisseur("Name", email));
        }
    }
}
