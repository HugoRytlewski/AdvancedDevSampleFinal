using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfFournisseurRepository : IFournisseurRepository
    {
        private static readonly List<Fournisseur> _fournisseurs = new();

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
            var fournisseur = _fournisseurs.FirstOrDefault(f => f.Id == id);
            if (fournisseur != null)
            {
                _fournisseurs.Remove(fournisseur);
            }
        }
    }
}
