using System;
using System.Collections.Generic;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    public interface ICommandeRepository
    {
        void Save(Commande commande);
        Commande? GetById(Guid id);
        IEnumerable<Commande> GetAll();
        IEnumerable<Commande> GetByClientId(Guid clientId);
        void Delete(Guid id);
    }
}
