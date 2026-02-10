using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using Xunit;
using System;

namespace AdvancedDevSample.Test.Domain.Entities
{
    public class ClientTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenValidArguments()
        {
            // Arrange
            string name = "John Doe";
            string email = "john@example.com";
            string address = "123 Main St";

            // Act
            var client = new Client(name, email, address);

            // Assert
            Assert.NotEqual(Guid.Empty, client.Id);
            Assert.Equal(name, client.Name);
            Assert.Equal(email, client.Email);
            Assert.Equal(address, client.Address);
        }

        [Theory]
        [InlineData("", "email@example.com", "Address")]
        [InlineData("Name", "", "Address")]
        [InlineData("Name", "email@example.com", "")]
        public void Constructor_ShouldThrowDomainException_WhenArgumentsInvalid(string name, string email, string address)
        {
            // Act & Assert
            Assert.Throws<DomainException>(() => new Client(name, email, address));
        }

        [Fact]
        public void UpdateAddress_ShouldUpdateAddress_WhenValid()
        {
            // Arrange
            var client = new Client("John", "john@example.com", "Old Address");
            string newAddress = "New Address";

            // Act
            client.UpdateAddress(newAddress);

            // Assert
            Assert.Equal(newAddress, client.Address);
        }

        [Fact]
        public void UpdateAddress_ShouldThrowException_WhenAddressInvalid()
        {
             // Arrange
            var client = new Client("John", "john@example.com", "Old Address");

            // Act & Assert
            Assert.Throws<DomainException>(() => client.UpdateAddress(""));
        }
    }
}
