using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Api.Tests.Integration
{
    public class InMemoryClientRepository : IClientRepository
    {
        private readonly List<Client> _clients = new();

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
            var client = GetById(id);
            if (client != null)
            {
                _clients.Remove(client);
            }
        }

        // Helper for tests
        public void Clear()
        {
            _clients.Clear();
        }

        public void Seed(Client client)
        {
            _clients.Add(client);
        }
    }
}
