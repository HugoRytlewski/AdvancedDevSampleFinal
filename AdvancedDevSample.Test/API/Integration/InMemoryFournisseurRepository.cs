using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Api.Tests.Integration
{
    public class InMemoryFournisseurRepository : IFournisseurRepository
    {
        private readonly List<Fournisseur> _fournisseurs = new();

        public void Save(Fournisseur fournisseur)
        {
            var existing = _fournisseurs.FirstOrDefault(f => f.Id == fournisseur.Id);
            if (existing != null)
            {
                _fournisseurs.Remove(existing);
            }
            _fournisseurs.Add(fournisseur);
        }

        public Fournisseur? GetById(Guid id)
        {
            return _fournisseurs.FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<Fournisseur> GetAll()
        {
            return _fournisseurs;
        }

        public void Delete(Guid id)
        {
            var fournisseur = GetById(id);
            if (fournisseur != null)
            {
                _fournisseurs.Remove(fournisseur);
            }
        }

        // Helper for tests
        public void Clear()
        {
            _fournisseurs.Clear();
        }

        public void Seed(Fournisseur fournisseur)
        {
            _fournisseurs.Add(fournisseur);
        }
    }
}
