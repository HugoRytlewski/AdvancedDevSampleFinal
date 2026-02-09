using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.ValueObjects;
using System;
using AdvancedDevSample.Domain.Interfaces;


namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        // Simulation de la persistance en mémoire (static pour persister entre les requêtes)
        private static readonly List<Product> _products = new();

        public void Save(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                _products.Remove(existingProduct);
            }
            _products.Add(product);
        }

        public Product GetById(Guid id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return product; // Peut retourner null, à gérer dans le service
        }

        public IEnumerable<Product> GetAll()
        {
            // Retourne uniquement les produits actifs (Soft Delete implicite si IsActive est utilisé ainsi)
            // Ou tout retourner et laisser le service filtrer. Ici on retourne tout.
            return _products;
        }

        public void Delete(Guid id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                // Soft delete
                product.Deactivate(); 
                // Pour cet exemple in-memory, on pourrait aussi le supprimer physiquement :
                // _products.Remove(product);
                // Mais gardons la logique de "Save" update pour la cohérence.
            }
        }
    }
}
