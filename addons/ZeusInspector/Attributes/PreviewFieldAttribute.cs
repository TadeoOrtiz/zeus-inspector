using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class PreviewFieldAttribute(int width) : InspectorAttribute
{
    public override void Apply(EditorProperty editor)
    {
        //foreach (var child in editor.GetChildren())
        //{
        //    child.QueueFree();
        //}
    }

}
