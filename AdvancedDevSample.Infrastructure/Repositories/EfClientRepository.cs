using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedDevSample.Infrastructure.Repositories
{
    public class EfClientRepository : IClientRepository
    {
        private static readonly List<Client> _clients = new();

        public void Save(Client client)
        {
            var existing = _clients.FirstOrDefault(c => c.Id == client.Id);
            if (existing != null)
            {
                _clients.Remove(existing);
            }
            _clients.Add(client);
        }

        public Client? GetById(Guid id)
        {
            return _clients.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Client> GetAll()
        {
            return _clients;
        }

        public void Delete(Guid id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                _clients.Remove(client);
            }
        }
    }
}
