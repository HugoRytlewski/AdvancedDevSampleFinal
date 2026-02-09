using System.ComponentModel.DataAnnotations;

namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Requête pour créer un nouveau produit.
    /// </summary>
    public class CreateProductRequest
    {
        /// <summary>
        /// Nom du produit.
        /// </summary>
        /// <example>iPhone 15 Pro</example>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Prix du produit (doit être strictement positif).
        /// </summary>
        /// <example>999.99</example>
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être strictement positif.")]
        public decimal Price { get; set; }

        /// <summary>
        /// Code de la devise (ex: EUR, USD, GBP).
        /// </summary>
        /// <example>EUR</example>
        public string Currency { get; set; } = "EUR";
    }
}
