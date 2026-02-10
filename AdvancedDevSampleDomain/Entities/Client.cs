using System;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Représente un client qui peut passer des commandes.
    /// </summary>
    public class Client
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }

        public Client(string name, string email, string address)
            : this(Guid.NewGuid(), name, email, address) { }

        public Client(Guid id, string name, string email, string address)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Le nom du client est obligatoire.");
            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("L'email du client est obligatoire.");
            if (string.IsNullOrWhiteSpace(address)) throw new DomainException("L'adresse du client est obligatoire.");

            Id = id;
            Name = name;
            Email = email;
            Address = address;
        }

        // Constructeur pour ORM
        protected Client()
        {
            Name = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
        }

        public void UpdateAddress(string newAddress)
        {
            if (string.IsNullOrWhiteSpace(newAddress)) throw new DomainException("La nouvelle adresse ne peut pas être vide.");
            Address = newAddress;
        }
    }
}
