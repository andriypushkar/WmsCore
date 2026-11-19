namespace Wms.Domain.ValueObjects;

public readonly record struct Dimensions(double WidthCm, double HeightCm, double DepthCm)
{
    public double VolumeCm3 => WidthCm * HeightCm * DepthCm;
}
