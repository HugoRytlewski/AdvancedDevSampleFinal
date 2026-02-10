using Xunit;
using AdvancedDevSample.Infrastructure.Repositories;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.ValueObjects;
using System;

namespace AdvancedDevSample.Test.Domain
{
    public class ProductEntityTests
    {
        [Fact]
        public void ToDomain_ShouldReturnValidProduct()
        {
            // Arrange
            var id = Guid.NewGuid();
            var price = 100m;
            var isActive = true;
            var entity = new ProductEntity(id, price, isActive);

            // Act
            var domain = entity.ToDomain();

            // Assert
            Assert.Equal(id, domain.Id);
            Assert.Equal(price, domain.Price.Value);
            Assert.Equal(isActive, domain.IsActive);
        }

        [Fact]
        public void ToDomain_ShouldReturnInactiveProduct_WhenEntityIsInactive()
        {
            // Arrange
            var id = Guid.NewGuid();
            var price = 100m;
            var isActive = false;
            var entity = new ProductEntity(id, price, isActive);

            // Act
            var domain = entity.ToDomain();

            // Assert
            Assert.Equal(id, domain.Id);
            Assert.Equal(price, domain.Price.Value);
            Assert.False(domain.IsActive);
        }

        [Fact]
        public void FromDomain_ShouldReturnValidEntity()
        {
            // Arrange
            var id = Guid.NewGuid();
            var price = new Price(100m, "EUR");
            var product = new Product(id, "Test Product", price);

            // Act
            var entity = ProductEntity.FromDomain(product);

            // Assert
            Assert.Equal(id, entity.Id);
            Assert.Equal(price.Value, entity.Price);
            Assert.Equal(product.IsActive, entity.IsActive);
        }

        [Fact]
        public void DefaultConstructor_ShouldInitializeProperties()
        {
            // Arrange & Act
            var entity = new ProductEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(Guid.Empty, entity.Id);
            Assert.Equal(0m, entity.Price);
            Assert.False(entity.IsActive);
        }
    }
}
