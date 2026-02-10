using System;
using System.Collections.Generic;
using AdvancedDevSample.Domain.Entities;

namespace AdvancedDevSample.Domain.Interfaces
{
    public interface IClientRepository
    {
        void Save(Client client);
        Client? GetById(Guid id);
        IEnumerable<Client> GetAll();
        void Delete(Guid id);
    }
}
