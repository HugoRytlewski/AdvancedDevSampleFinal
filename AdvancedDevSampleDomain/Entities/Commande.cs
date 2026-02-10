using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.Entities
{
    public enum CommandeStatus
    {
        Created,
        Validated,
        Shipped,
        Delivered,
        Cancelled
    }

    /// <summary>
    /// Représente une commande passée par un client.
    /// </summary>
    public class Commande
    {
        public Guid Id { get; private set; }
        public Guid ClientId { get; private set; }
        public DateTime Date { get; private set; }
        public CommandeStatus Status { get; private set; }
        private readonly List<LigneCommande> _lignes = new();
        public IReadOnlyCollection<LigneCommande> Lignes => _lignes.AsReadOnly();

        public Commande(Guid clientId) : this(Guid.NewGuid(), clientId, DateTime.UtcNow, CommandeStatus.Created) { }

        public Commande(Guid id, Guid clientId, DateTime date, CommandeStatus status)
        {
            if (clientId == Guid.Empty) throw new DomainException("Le client est obligatoire.");

            Id = id;
            ClientId = clientId;
            Date = date;
            Status = status;
        }

        // Constructeur pour ORM
        protected Commande() { }

        public void AddItem(Guid productId, int quantity, Price unitPrice)
        {
            if (Status != CommandeStatus.Created)
            {
                throw new DomainException("Impossible de modifier une commande qui n'est pas en statut 'Created'.");
            }

            var existingLine = _lignes.FirstOrDefault(l => l.ProductId == productId);
            if (existingLine != null)
            {
                // Dans une implémentation plus complexe, on pourrait fusionner les lignes ou mettre à jour la quantité.
                // Pour simplifier ici, on rejette l'ajout si le produit est déjà présent, ou on pourrait le retirer et le remettre.
                throw new DomainException("Ce produit est déjà dans la commande. Veuillez ajuster la quantité ou supprimer la ligne.");
            }

            _lignes.Add(new LigneCommande(productId, quantity, unitPrice));
        }

        public void RemoveItem(Guid productId)
        {
            if (Status != CommandeStatus.Created)
            {
                throw new DomainException("Impossible de modifier une commande qui n'est pas en statut 'Created'.");
            }

            var line = _lignes.FirstOrDefault(l => l.ProductId == productId);
            if (line != null)
            {
                _lignes.Remove(line);
            }
        }

        public decimal CalculateTotal()
        {
            return _lignes.Sum(l => l.Total);
        }

        public void Validate()
        {
            if (!_lignes.Any()) throw new DomainException("Une commande doit contenir au moins une ligne pour être validée.");
            if (Status != CommandeStatus.Created) throw new DomainException("Seule une commande 'Created' peut être validée.");
            
            Status = CommandeStatus.Validated;
        }

        public void Cancel()
        {
            if (Status == CommandeStatus.Shipped || Status == CommandeStatus.Delivered)
            {
                throw new DomainException("Impossible d'annuler une commande expédiée ou livrée.");
            }
            Status = CommandeStatus.Cancelled;
        }
    }
}
