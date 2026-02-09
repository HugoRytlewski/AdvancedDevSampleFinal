using System.ComponentModel.DataAnnotations;

namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Requête pour modifier le prix d'un produit.
    /// </summary>
    public class ChangePriceRequest
    {
        /// <summary>
        /// Nouveau prix du produit (doit être strictement positif).
        /// </summary>
        /// <example>149.99</example>
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être strictement positif.")]
        public decimal NewPrice { get; set; }
    }
}