using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using System.Collections.Concurrent;

namespace AdvancedDevSample.Api.Tests.Integration
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly ConcurrentDictionary<Guid, Product> _products = new();

        public void Save(Product product)
        {
            _products[product.Id] = product;
        }

        public Product? GetById(Guid id)
        {
            _products.TryGetValue(id, out var product);
            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            return _products.Values;
        }

        public void Delete(Guid id)
        {
            _products.TryRemove(id, out _);
        }

        // Helper for tests
        public void Data(IEnumerable<Product> products)
        {
            Clear();
            foreach (var p in products)
            {
                Save(p);
            }
        }

        public void Clear()
        {
            _products.Clear();
        }

        public void Seed(Product product)
        {
            Save(product);
        }
    }
}
