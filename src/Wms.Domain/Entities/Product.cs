using Wms.Domain.ValueObjects;

namespace Wms.Domain.Entities
{
    public record class Product
    {
        public required string Sku { get; init; }
        public required string Name { get; init; }
        public required double WeightKg { get; init; }
        public required Dimensions Size {  get; init; }
        public string? Description { get; init; }
    }
}
