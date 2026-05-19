using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class CustomEditorAttribute : Attribute
{
    public Type EditorType { get; }

    public CustomEditorAttribute(Type editorType)
    {
        EditorType = editorType;
    }
}