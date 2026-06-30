using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class CustomEditor<T> : InspectorAttribute where T : EditorProperty
{
    public override EditorProperty CreateEditor(Variant.Type type, string propName, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        return Activator.CreateInstance<T>();
    }


}
