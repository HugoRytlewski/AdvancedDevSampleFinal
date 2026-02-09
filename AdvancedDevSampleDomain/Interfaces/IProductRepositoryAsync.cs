using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    /// <summary>
    /// Contrat asynchrone de persistance pour les produits (pour tests et implémentations EF/infra).
    /// </summary>
    public interface IProductRepositoryAsync
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> ListAllAsync();
        Task AddAsync(Product product);
        Task SaveAsync(Product product);
        Task RemoveAsync(Guid id);
    }
}