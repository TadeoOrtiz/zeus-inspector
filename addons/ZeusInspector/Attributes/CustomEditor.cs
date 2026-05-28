using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class CustomEditor<T> : InspectorAttribute where T : EditorProperty
{
    public override EditorProperty CreateEditor(Variant.Type type, string propName, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        var e = Activator.CreateInstance<T>();
        return e;
    }

    public override void ParseEditor(EditorProperty editor)
    {
        //editor.Label = "";
        //editor.DrawLabel = false;
        //editor.Selectable = true;
    }

}
