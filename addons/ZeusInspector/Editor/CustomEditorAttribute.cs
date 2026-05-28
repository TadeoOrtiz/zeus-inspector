using System;

namespace ZeusInspector.Editor;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class CustomEditorAttribute(Type editorType) : Attribute
{
    public Type EditorType { get; } = editorType;
}