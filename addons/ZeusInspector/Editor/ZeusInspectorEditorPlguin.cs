using System.Collections.Generic;
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

        customInspectors.TryAdd(propName.Capitalize(), attributes);
        foreach (var attr in attributes)
        {
            var editor = attr.CreateEditor(type, propName, hintType, hintString, usageFlags, wide);
            if (editor != null)
            {
                AddPropertyEditor(propName, editor);
                return true;
            }
        }
        return false;
    }



    public override void _ParseEnd(GodotObject @object)
    {
        var inspector = EditorInterface.Singleton.GetInspector();

        GroupAttribute.Cleanup(inspector);
        ParseEditors(inspector);
    }

    private void ParseEditors(Node node)
    {
        foreach (var child in node.GetChildren())
        {

            if (child is EditorProperty editorProperty)
            {
                if (customInspectors.TryGetValue(editorProperty.Label, out var attributes))
                {
                    foreach (var attr in attributes)
                    {
                        attr.ParseEditor(editorProperty);
                    }
                }
            }

            ParseEditors(child);
        }

    }

}
