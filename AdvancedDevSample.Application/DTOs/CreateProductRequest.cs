namespace AdvancedDevSample.Application.DTOs
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = "EUR"; // Valeur par défaut
    }
}
