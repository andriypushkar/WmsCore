using System;
using System.Collections.Generic;
using System.Text;
using Wms.Domain.Enums;

namespace Wms.Domain.Entities
{
    public class WarehouseSlot
    {
        public required string SlotCode { get; init; }
        public required double MaxWeightKg { get; init; }
        public required double MaxVolumeCm3 { get; init; }
        public SlotStatus Status { get; private set; } = SlotStatus.Available;

        private readonly List<Product> _storedProducts = [];
        public IReadOnlyCollection<Product> StoredProducts => _storedProducts.AsReadOnly();

        public double CurrentWeightKg => _storedProducts.Sum(x => x.WeightKg);
        public double CurrentVolumeCm3 => _storedProducts.Sum(x => x.Size.VolumeCm3);
        public bool CanFit(Product product) =>
            (Status, product.WeightKg + CurrentWeightKg <= MaxWeightKg , product.Size.VolumeCm3 + CurrentVolumeCm3 <= MaxVolumeCm3) switch
            {
                (SlotStatus.Available, true, true) => true,
                _ => false
            };

    }
}
