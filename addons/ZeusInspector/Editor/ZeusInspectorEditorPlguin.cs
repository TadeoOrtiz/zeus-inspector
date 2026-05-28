using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using ZeusInspector.Attributes;

namespace ZeusInspector.Editor;

[Tool]
public partial class ZeusInspectorEditorPlguin : EditorInspectorPlugin
{

    private Dictionary<string, List<InspectorAttribute>> customInspectors = [];

    public override bool _CanHandle(GodotObject @object)
    {
        customInspectors.Clear();
        return true;
    }


    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string propName, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        var attributes = AttributeResolver.GetAttributes(@object, propName);
        customInspectors.TryAdd(propName, attributes);

        return false;
    }

    public override void _ParseEnd(GodotObject @object)
    {
        var inspector = EditorInterface.Singleton.GetInspector();

        PrintRecursiveChilds(inspector);
    }

    private void PrintRecursiveChilds(Node node)
    {
        foreach (var child in node.GetChildren())
        {

            if (child is EditorProperty editorProperty)
            {
                if (customInspectors.TryGetValue(editorProperty.Label, out var attributes))
                {
                    GD.Print($"EditorProperty: {editorProperty.Label}");
                    foreach (var attr in attributes)
                    {
                        GD.Print($"  - Attribute: {attr.GetType().Name}");
                        attr.Apply(editorProperty);
                    }
                }
            }

            PrintRecursiveChilds(child);
        }
    }

}
