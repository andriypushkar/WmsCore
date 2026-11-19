using System;
using System.Collections.Generic;
using System.Text;

namespace Wms.Domain
{
    public readonly record struct Dimensions
    {
        public double WidthCm { get; init; }
        public double HeightCm { get; init; }
        public double DepthCm { get; init; }

        public double VolumeCm3 => WidthCm * HeightCm * DepthCm;
    }
}
