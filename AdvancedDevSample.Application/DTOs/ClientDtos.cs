using System;

namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Représente un client.
    /// </summary>
    public record ClientDto(
        /// <summary>L'identifiant unique du client.</summary>
        Guid Id, 
        /// <summary>Le nom complet du client.</summary>
        string Name, 
        /// <summary>L'adresse email du client.</summary>
        string Email, 
        /// <summary>L'adresse postale du client.</summary>
        string Address);

    /// <summary>
    /// Requête de création d'un client.
    /// </summary>
    public record CreateClientRequest(
        /// <summary>Le nom complet du nouveau client.</summary>
        string Name, 
        /// <summary>L'adresse email du nouveau client.</summary>
        string Email, 
        /// <summary>L'adresse postale du nouveau client.</summary>
        string Address);

    /// <summary>
    /// Requête de mise à jour de l'adresse d'un client.
    /// </summary>
    public record UpdateClientAddressRequest(
        /// <summary>La nouvelle adresse postale.</summary>
        string NewAddress);
}
