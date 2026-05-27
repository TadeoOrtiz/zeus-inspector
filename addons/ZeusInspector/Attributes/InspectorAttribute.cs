using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public abstract class InspectorAttribute : Attribute
{
    public abstract void Apply(EditorProperty editor);

    public virtual AttributeResult Apply(EditorProperty editor, string name, GodotObject obj, EditorInspectorPlugin inspector)
    {
        Apply(editor);
        return AttributeResult.Continue;
    }

    public virtual void OnParseBegin(GodotObject obj) { }
    public virtual void OnParseEnd(GodotObject obj) { }
}

public enum AttributeResult
{
    Continue,
    Handled,
    SkipProperty
}
