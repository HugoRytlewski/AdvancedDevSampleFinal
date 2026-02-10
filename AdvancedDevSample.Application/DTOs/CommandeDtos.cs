using System;
using System.Collections.Generic;

namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Représente une commande.
    /// </summary>
    public record CommandeDto(
        /// <summary>L'identifiant unique de la commande.</summary>
        Guid Id, 
        /// <summary>L'identifiant du client ayant passé la commande.</summary>
        Guid ClientId, 
        /// <summary>La date de création de la commande.</summary>
        DateTime Date, 
        /// <summary>Le statut actuel de la commande (ex: Created, Validated).</summary>
        string Status, 
        /// <summary>La liste des lignes de la commande.</summary>
        List<LigneCommandeDto> Lignes, 
        /// <summary>Le montant total de la commande.</summary>
        decimal TotalAmount);

    /// <summary>
    /// Représente une ligne de commande.
    /// </summary>
    public record LigneCommandeDto(
        /// <summary>L'identifiant du produit.</summary>
        Guid ProductId, 
        /// <summary>Le nom du produit.</summary>
        string ProductName, 
        /// <summary>La quantité commandée.</summary>
        int Quantity, 
        /// <summary>Le prix unitaire du produit.</summary>
        decimal UnitPrice, 
        /// <summary>La devise du prix.</summary>
        string Currency);

    /// <summary>
    /// Requête de création d'une commande.
    /// </summary>
    public record CreateCommandeRequest(
        /// <summary>L'identifiant du client pour lequel créer la commande.</summary>
        Guid ClientId);

    /// <summary>
    /// Requête d'ajout d'un article à une commande.
    /// </summary>
    public record AddItemRequest(
        /// <summary>L'identifiant du produit à ajouter.</summary>
        Guid ProductId, 
        /// <summary>La quantité à ajouter.</summary>
        int Quantity);
}
