using System;
using System.Collections.Generic;
using System.Text;

namespace Wms.Domain.Entities
{
    public record class Product
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public double WeightKg { get; set; }
        public Dimensions Size {  get; set; }
        public string? Description { get; set; }
    }
}
