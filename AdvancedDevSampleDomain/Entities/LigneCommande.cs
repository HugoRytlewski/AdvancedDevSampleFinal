using System;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Représente une ligne au sein d'une commande.
    /// </summary>
    public class LigneCommande
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public Price UnitPrice { get; private set; }

        public LigneCommande(Guid productId, int quantity, Price unitPrice)
        {
            if (productId == Guid.Empty) throw new DomainException("L'ID du produit est obligatoire.");
            if (quantity <= 0) throw new DomainException("La quantité doit être supérieure à zéro.");

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        // Constructeur pour ORM
        protected LigneCommande() { }

        public decimal Total => UnitPrice.Value * Quantity;
    }
}
