using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Domain.ValueObjects
{
    /// <summary>
    /// Value Object représentant un prix strictement positif.
    /// </summary>
    public readonly record struct Price
    {
        public decimal Value { get; init; }
        public string Currency { get; init; }

        public Price(decimal value, string currency = "EUR")
        {
            if (value <= 0m)
            {
                throw new DomainException("Un prix doit être strictement positif.");
            }
            if (string.IsNullOrWhiteSpace(currency))
            {
                 throw new DomainException("La devise est obligatoire.");
            }

            Value = value;
            Currency = currency;
        }

        public override string ToString() => $"{Value:F2} {Currency}";
    }
}