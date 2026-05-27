using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class HideLabelAttribute() : InspectorAttribute
{
    public override void Apply(EditorProperty editor)
    {
        editor.DrawLabel = false;
        editor.Label = string.Empty;
    }

}
