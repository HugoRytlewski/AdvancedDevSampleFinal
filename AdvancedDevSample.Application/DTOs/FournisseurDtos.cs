using System;

namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Représente un fournisseur.
    /// </summary>
    public record FournisseurDto(
        /// <summary>L'identifiant unique du fournisseur.</summary>
        Guid Id, 
        /// <summary>Le nom du fournisseur.</summary>
        string Name, 
        /// <summary>L'email de contact du fournisseur.</summary>
        string ContactEmail);

    /// <summary>
    /// Requête de création d'un fournisseur.
    /// </summary>
    public record CreateFournisseurRequest(
        /// <summary>Le nom du nouveau fournisseur.</summary>
        string Name, 
        /// <summary>L'email de contact du nouveau fournisseur.</summary>
        string ContactEmail);
}
