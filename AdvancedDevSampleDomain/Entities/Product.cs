using System;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Represente un produit vendable.
    /// </summary>
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public Price Price { get; private set; }
        public bool IsActive { get; private set; }

        public Product(string name, Price price) : this(Guid.NewGuid(), name, price) { }

        public Product(Guid id, string name, Price price)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = string.IsNullOrWhiteSpace(name) ? throw new DomainException("Le nom est obligatoire.") : name;
            Price = price;
            IsActive = true;
        }

        public Product(Price price) : this("Produit sans nom", price) { }

        public Product(Guid id, Price price, bool isActive)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = "Produit sans nom";
            Price = price;
            IsActive = isActive;
        }

        public Product(Guid id, Price price) : this(id, "Produit sans nom", price) { }

        // Constructeur requis par certains ORMs ; protege pour empecher l'utilisation publique.
        protected Product()
        {
            Name = string.Empty;
            Price = new Price(0, "EUR");
            IsActive = true;
        }

        public void ChangePrice(Price newPrice)
        {
            if (!IsActive)
            {
                throw new DomainException("Le produit est inactif.");
            }
            Price = newPrice;
        }

        public void ChangePrice(decimal newPriceValue)
        {
            if (!IsActive)
            {
                throw new DomainException("Le produit est inactif.");
            }
            Price = new Price(newPriceValue, Price.Currency);
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}