using System;
using Godot;

namespace ZeusInspector.Editor;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class CustomDockAttribute(Type editorType, EditorDock.DockSlot dockSlot = EditorDock.DockSlot.Bottom) : Attribute
{
    public Type EditorType { get; } = editorType;
    public EditorDock.DockSlot DockSlot { get; } = dockSlot;
}