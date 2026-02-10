using System;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Domain.Entities
{
    /// <summary>
    /// Représente un fournisseur de produits.
    /// </summary>
    public class Fournisseur
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string ContactEmail { get; private set; }

        public Fournisseur(string name, string contactEmail)
            : this(Guid.NewGuid(), name, contactEmail) { }

        public Fournisseur(Guid id, string name, string contactEmail)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Le nom du fournisseur est obligatoire.");
            if (string.IsNullOrWhiteSpace(contactEmail)) throw new DomainException("L'email de contact est obligatoire.");

            Id = id;
            Name = name;
            ContactEmail = contactEmail;
        }

        // Constructeur pour ORM
        protected Fournisseur()
        {
            Name = string.Empty;
            ContactEmail = string.Empty;
        }
    }
}
