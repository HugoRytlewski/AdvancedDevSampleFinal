using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Application.Services
{
    public class CommandeService
    {
        private readonly ICommandeRepository _commandeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;

        public CommandeService(ICommandeRepository commandeRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _commandeRepository = commandeRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
        }

        public CommandeDto CreateCommande(CreateCommandeRequest request)
        {
            var client = _clientRepository.GetById(request.ClientId);
            if (client == null) throw new DomainException("Client non trouvé.");

            var commande = new Commande(client.Id);
            _commandeRepository.Save(commande);
            return ToDto(commande);
        }

        public CommandeDto getCommande(Guid id)
        {
            var commande = _commandeRepository.GetById(id);
            if (commande == null) throw new DomainException("Commande non trouvée.");
            return ToDto(commande);
        }

        public void AddItem(Guid commandeId, AddItemRequest request)
        {
            var commande = _commandeRepository.GetById(commandeId);
            if (commande == null) throw new DomainException("Commande non trouvée.");

            var product = _productRepository.GetById(request.ProductId);
            if (product == null) throw new DomainException("Produit non trouvé.");

            commande.AddItem(product.Id, request.Quantity, product.Price);
            _commandeRepository.Save(commande); // Save changes if repository implementation requires it (persist state)
        }

        public void RemoveItem(Guid commandeId, Guid productId)
        {
             var commande = _commandeRepository.GetById(commandeId);
             if (commande == null) throw new DomainException("Commande non trouvée.");
             
             commande.RemoveItem(productId);
             _commandeRepository.Save(commande);
        }

        public void ValidateCommande(Guid commandeId)
        {
            var commande = _commandeRepository.GetById(commandeId);
             if (commande == null) throw new DomainException("Commande non trouvée.");
            
            commande.Validate();
            _commandeRepository.Save(commande);
        }

        public void CancelCommande(Guid commandeId)
        {
            var commande = _commandeRepository.GetById(commandeId);
             if (commande == null) throw new DomainException("Commande non trouvée.");
            
            commande.Cancel();
            _commandeRepository.Save(commande);
        }

        private CommandeDto ToDto(Commande commande)
        {
            // Note: In a real app we might need to fetch product names for LigneCommandeDto if we don't store them.
            // For simplicity here, we might just put "Product {Id}" or rely on a more complex fetching strategy.
            // Let's iterate and try to fetch names if possible or keep it simple.
            // Since repositories are injected, we CAN fetch product names.
            
            var lignesDtos = commande.Lignes.Select(l => 
            {
                var product = _productRepository.GetById(l.ProductId);
                string productName = product?.Name ?? "Produit inconnu";
                return new LigneCommandeDto(l.ProductId, productName, l.Quantity, l.UnitPrice.Value, l.UnitPrice.Currency);
            }).ToList();

            return new CommandeDto(
                commande.Id, 
                commande.ClientId, 
                commande.Date, 
                commande.Status.ToString(), 
                lignesDtos, 
                commande.CalculateTotal());
        }
    }
}
