using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Api.Tests.Integration
{
    /// <summary>
    /// In-memory implementation of IProductRepository for integration testing.
    /// </summary>
    public class InMemoryProductRepositoryAsync : IProductRepository
    {
        private readonly Dictionary<Guid, Product> _products = new();

        public void Save(Product product)
        {
            _products[product.Id] = product;
        }

        public Product GetById(Guid id)
        {
            _products.TryGetValue(id, out var product);
            return product!;
        }

        public IEnumerable<Product> GetAll()
        {
            return _products.Values;
        }

        public void Delete(Guid id)
        {
            _products.Remove(id);
        }

        // Helper methods for testing
        public void Seed(Product product) => Save(product);
        public void Clear() => _products.Clear();
    }
}
