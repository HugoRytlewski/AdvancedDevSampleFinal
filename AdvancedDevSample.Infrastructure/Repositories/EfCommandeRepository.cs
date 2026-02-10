using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfCommandeRepository : ICommandeRepository
    {
        private static readonly List<Commande> _commandes = new();

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

        public IEnumerable<Commande> GetAll()
        {
            return _commandes;
        }

        public IEnumerable<Commande> GetByClientId(Guid clientId)
        {
            return _commandes.Where(c => c.ClientId == clientId);
        }

        public void Delete(Guid id)
        {
            var commande = _commandes.FirstOrDefault(c => c.Id == id);
            if (commande != null)
            {
                _commandes.Remove(commande);
            }
        }
    }
}
