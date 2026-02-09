using System;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.Entities
{
    public class Product
    {
        private Guid guid;

        /// <summary>
        /// Représente un produit vendable.
        /// </summary>
        public Guid Id { get; private set; } // Identité
        public string Name { get; private set; } // Nouveau champ Name
        public Price Price { get; private set; } // Invariant encapsulé dans Price
        public bool IsActive { get; private set; } // true par défaut

        public Product(string name, Price price) : this(Guid.NewGuid(), name, price) { }

        public Product(Guid id, string name, Price price)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = string.IsNullOrWhiteSpace(name) ? throw new DomainException("Le nom est obligatoire.") : name;
            Price = price; // Price valide par construction
            IsActive = true;
        }

        public Product(Price price) : this("Produit sans nom", price) { }

        public Product(Guid id, Price price, bool IsActive)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = "Produit sans nom";
            Price = price; // Price valide par construction
            this.IsActive = IsActive;
        }

        // Constructeur requis par certains ORMs ; protégé pour empêcher l'utilisation publique.
        protected Product()
        {
            IsActive = true;
        }

        public Product(Guid guid, Price price)
        {
            this.guid = guid;
            Price = price;
             Name = "Produit sans nom";
             IsActive = true;
        }

        public void ChangePrice(Price newPrice)
        {
            // Règle métier : le produit ne doit pas être inactif
            if (!IsActive)
            {
                throw new DomainException("Le produit est inactif.");
            }

            // Invariant déjà garanti par Price
            Price = newPrice;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

        public void ChangePrice(decimal newPriceValue)
        {
            // Implementation temporaire pour éviter l'erreur NotImplementedException si utilisée
             if (!IsActive)
            {
                throw new DomainException("Le produit est inactif.");
            }
             // On assume EUR par défaut pour simplifier ici, idéalement il faudrait passer la currency
             Price = new Price(newPriceValue, Price.Currency);
        }
    }
}