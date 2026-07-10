using System;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class InspectorRangeAttribute : Attribute
{
    public double Min { get; }
    public double Max { get; }
    public double Step { get; }

    public InspectorRangeAttribute(double min, double max, double step = 1)
    {
        Min = min;
        Max = max;
        Step = step;
    }
}
