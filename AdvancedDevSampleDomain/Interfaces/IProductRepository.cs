using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    public interface IProductRepository
    {
        void Save(Product product);
        Product? GetById(Guid id);
        IEnumerable<Product> GetAll();
        void Delete(Guid id);
    }
}
