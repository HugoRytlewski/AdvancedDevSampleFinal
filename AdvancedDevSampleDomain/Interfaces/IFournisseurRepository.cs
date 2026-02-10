using System;
using System.Collections.Generic;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    public interface IFournisseurRepository
    {
        void Save(Fournisseur fournisseur);
        Fournisseur? GetById(Guid id);
        IEnumerable<Fournisseur> GetAll();
        void Delete(Guid id);
    }
}
