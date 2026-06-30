using System.Collections.Generic;
using Godot;

namespace ZeusInspector.Editor;

public class CustomDock
{
    public GodotObject Target { get; set; }
    public virtual Control CreateInspectorGUI() => new();
    public virtual EditorProperty CreatePropertyEditor(string name) => null;
}
