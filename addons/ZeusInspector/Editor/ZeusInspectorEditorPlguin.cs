using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ZeusInspector.Attributes;

namespace ZeusInspector.Editor;

[Tool]
public partial class ZeusInspectorEditorPlguin : EditorInspectorPlugin
{

    private readonly Dictionary<string, List<InspectorAttribute>> customInspectors = [];
    private readonly Dictionary<string, Control> groups = [];

    public override bool _CanHandle(GodotObject @object)
    {
        groups.Clear();
        customInspectors.Clear();
        return true;
    }



    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string propName, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        GD.Print($"Parsing property '{propName}' for {@object.GetType().Name}");
        if (usageFlags == PropertyUsageFlags.Editor && hintType == PropertyHint.None)
        {
            AddCustomControl(new EditorInspectorFolder());
            return true;
        }
        return false;
    }

    public override void _ParseBegin(GodotObject @object)
    {
        GD.Print($"Parsing {@object.GetType().Name} with ZeusInspectorEditorPlugin");
        var inspector = EditorInterface.Singleton.GetInspector();
        //inspector.PrintTreePretty();
    }

    public override void _ParseCategory(GodotObject @object, string category)
    {
        GD.Print($"Parsing category '{category}' for {@object.GetType().Name}");
        var inspector = EditorInterface.Singleton.GetInspector();
        //inspector.PrintTreePretty();
    }


    public override void _ParseGroup(GodotObject @object, string groupName)
    {
        GD.Print($"Parsing group '{groupName}' for {@object.GetType().Name}");

        groups.TryAdd(groupName, GetInspectorSection(EditorInterface.Singleton.GetInspector()));

    }


    public override void _ParseEnd(GodotObject @object)
    {
        GD.Print($"Finished parsing {@object.GetType().Name}");
        ParseGroups(@object);
        //inspector.PrintTreePretty();
        //GroupAttribute.Cleanup(inspector);
        //ParseEditors(inspector);
    }

    private void ParseGroups(GodotObject @object)
    {
        foreach (var prop in @object.GetPropertyList())
        {
            if ((PropertyUsageFlags)(int)prop["usage"] == PropertyUsageFlags.Group)
            {
                string groupName = (string)prop["name"];
                string hintString = (string)prop["hint_string"];
                var hintParts = hintString.Split(',');

                if (hintParts.Length >= 3)
                {
                    groups.TryGetValue(groupName, out var groupControl);
                    if (groupControl != null)
                    {
                        _ = int.TryParse(hintParts[2], out var groupOrientation);
                        _ = Enum.TryParse<ZExportGroupAttribute.Orientation>(groupOrientation.ToString(), out var orientation);
                        GD.Print($"Group '{groupName}' has orientation: {orientation}");


                        var vbox = groupControl.GetChild(0);
                        if (orientation == ZExportGroupAttribute.Orientation.Horizontal)
                        {
                            GD.Print(groupControl);
                            var hbox = new HBoxContainer();
                            vbox.AddChild(hbox);
                            hbox.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                            foreach (var child in vbox.GetChildren().Cast<Control>())
                            {
                                if (child == hbox) continue;
                                child.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                                child.SizeFlagsVertical = Control.SizeFlags.ShrinkBegin;
                                child.Reparent(hbox);
                            }

                        }

                    }
                }
            }
        }
    }

    private Control GetInspectorSection(Node node)
    {
        foreach (var child in node.GetChildren())
        {
            if (child.GetClass() == "EditorInspectorSection")
            {
                return (Control)child;
            }

            var result = GetInspectorSection(child);
            if (result != null)
                return result;
        }
        return null;
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
