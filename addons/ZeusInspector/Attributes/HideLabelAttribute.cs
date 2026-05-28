using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class HideLabelAttribute() : InspectorAttribute
{
    public override void ParseEditor(EditorProperty editor)
    {
        editor.DrawLabel = false;
        editor.Label = " ";
    }

}
