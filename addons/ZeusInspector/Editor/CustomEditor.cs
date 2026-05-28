using System.Collections.Generic;
using Godot;

namespace ZeusInspector.Editor;

public class CustomInspector
{
    public GodotObject Target { get; set; }

    /// <summary>
    /// Indica si el GUI personalizado ya fue añadido al inspector.
    /// </summary>
    public bool WasGuiAdded { get; set; }

    /// <summary>
    /// Lista de nombres de propiedades que se deben ocultar en el inspector.
    /// </summary>
    public List<string> HiddenProperties { get; } = new();

    public virtual Control CreateInspectorGUI() => new Control();
    public virtual EditorProperty CreatePropertyEditor(string name) => null;


    /// <summary>
    /// Determina si una propiedad específica debe ocultarse.
    /// </summary>
    public virtual bool ShouldHideProperty(string name)
    {
        return HiddenProperties.Contains(name);
    }
}
