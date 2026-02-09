using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Application.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
    }
}
