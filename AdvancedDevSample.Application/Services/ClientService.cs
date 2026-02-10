using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Application.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public ClientDto CreateClient(CreateClientRequest request)
        {
            var client = new Client(request.Name, request.Email, request.Address);
            _clientRepository.Save(client);
            return ToDto(client);
        }

        public ClientDto GetClient(Guid id)
        {
            var client = _clientRepository.GetById(id);
            if (client == null) throw new DomainException("Client non trouvé.");
            return ToDto(client);
        }

        public IEnumerable<ClientDto> GetAllClients()
        {
            return _clientRepository.GetAll().Select(ToDto);
        }

        public void UpdateClientAddress(Guid id, string newAddress)
        {
            var client = _clientRepository.GetById(id);
            if (client == null) throw new DomainException("Client non trouvé.");
            
            client.UpdateAddress(newAddress);
            _clientRepository.Save(client);
        }

        public void DeleteClient(Guid id)
        {
             _clientRepository.Delete(id);
        }

        private static ClientDto ToDto(Client client)
        {
            return new ClientDto(client.Id, client.Name, client.Email, client.Address);
        }
    }
}
