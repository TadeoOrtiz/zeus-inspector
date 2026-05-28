using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class ButtonAttribute(string name = "") : InspectorAttribute
{

    private string Name { get; } = name;
    public override EditorProperty CreateEditor(Variant.Type type, string propName, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        var editor = new EditorProperty();

        return base.CreateEditor(type, propName, hintType, hintString, usageFlags, wide);
    }

}


