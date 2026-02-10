using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Api.Tests.Integration
{
    public class InMemoryCommandeRepository : ICommandeRepository
    {
        private readonly List<Commande> _commandes = new();

        public void Save(Commande commande)
        {
            var existing = _commandes.FirstOrDefault(c => c.Id == commande.Id);
            if (existing != null)
            {
                _commandes.Remove(existing);
            }
            _commandes.Add(commande);
        }

        public Commande? GetById(Guid id)
        {
            return _commandes.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Commande> GetByClientId(Guid clientId)
        {
            return _commandes.Where(c => c.ClientId == clientId);
        }

        public IEnumerable<Commande> GetAll()
        {
            return _commandes;
        }

        public void Delete(Guid id)
        {
             var existing = _commandes.FirstOrDefault(c => c.Id == id);
            if (existing != null)
            {
                _commandes.Remove(existing);
            }
        }

        // Helper for tests
        public void Clear()
        {
            _commandes.Clear();
        }

        public void Seed(Commande commande)
        {
            _commandes.Add(commande);
        }
    }
}
