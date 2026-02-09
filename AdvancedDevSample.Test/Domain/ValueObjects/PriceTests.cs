using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;
using Xunit;

namespace AdvancedDevSample.Test.Domain.ValueObjects
{
    public class PriceTests
    {
        [Fact]
        public void Constructor_Should_Create_Price_With_Valid_Value()
        {
            // Arrange & Act
            var price = new Price(99.99m, "EUR");

            // Assert
            Assert.Equal(99.99m, price.Value);
            Assert.Equal("EUR", price.Currency);
        }

        [Fact]
        public void Constructor_Should_Use_EUR_As_Default_Currency()
        {
            // Arrange & Act
            var price = new Price(50m);

            // Assert
            Assert.Equal("EUR", price.Currency);
        }

        [Fact]
        public void Constructor_Should_Throw_When_Value_Is_Negative()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() => new Price(-10m));
            Assert.Equal("Un prix doit être strictement positif.", ex.Message);
        }

        [Fact]
        public void Constructor_Should_Throw_When_Value_Is_Zero()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() => new Price(0m));
            Assert.Equal("Un prix doit être strictement positif.", ex.Message);
        }

        [Fact]
        public void Constructor_Should_Throw_When_Currency_Is_Empty()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() => new Price(10m, ""));
            Assert.Equal("La devise est obligatoire.", ex.Message);
        }

        [Fact]
        public void Constructor_Should_Throw_When_Currency_Is_Whitespace()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<DomainException>(() => new Price(10m, "   "));
            Assert.Equal("La devise est obligatoire.", ex.Message);
        }

        [Fact]
        public void ToString_Should_Return_Formatted_Price()
        {
            // Arrange
            var price = new Price(123.45m, "USD");

            // Act
            var result = price.ToString();

            // Assert - Use Contains for locale-independence (decimal separator may vary)
            Assert.Contains("123", result);
            Assert.Contains("45", result);
            Assert.Contains("USD", result);
        }
    }
}
