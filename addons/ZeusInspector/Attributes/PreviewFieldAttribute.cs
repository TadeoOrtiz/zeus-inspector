using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class PreviewFieldAttribute(int size) : InspectorAttribute
{
    private int Size { get; } = size;

    public override EditorProperty CreateEditor(Variant.Type type, string propName, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        var previewResource = new PreviewResource(hintString, Size);
        return previewResource;
    }

}
