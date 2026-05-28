using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public abstract class InspectorAttribute : Attribute
{
    public virtual void ParseEditor(EditorProperty editor)
    {

    }

    public virtual EditorProperty CreateEditor(Variant.Type type, string propName, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        return null;
    }



}
