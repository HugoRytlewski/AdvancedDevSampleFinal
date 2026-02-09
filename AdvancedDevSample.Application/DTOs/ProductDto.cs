namespace AdvancedDevSample.Application.DTOs
{
    /// <summary>
    /// Représentation d'un produit pour les réponses API.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Identifiant unique du produit.
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid Id { get; set; }

        /// <summary>
        /// Nom du produit.
        /// </summary>
        /// <example>iPhone 15 Pro</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Prix du produit.
        /// </summary>
        /// <example>999.99</example>
        public decimal Price { get; set; }

        /// <summary>
        /// Code de la devise.
        /// </summary>
        /// <example>EUR</example>
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Indique si le produit est actif.
        /// </summary>
        /// <example>true</example>
        public bool IsActive { get; set; }
    }
}
