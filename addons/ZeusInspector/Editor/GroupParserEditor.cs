#if TOOLS
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ZeusInspector.Attributes;

namespace ZeusInspector.Editor;

[Tool]
public partial class GroupParserEditor : EditorInspectorPlugin
{

  private readonly Dictionary<string, Control> groups = [];

  public override bool _CanHandle(GodotObject @object)
  {
    groups.Clear();
    return true;
  }


  public override void _ParseGroup(GodotObject @object, string groupName)
  {
    groups.TryAdd(groupName, GetInspectorSection(EditorInterface.Singleton.GetInspector()));
  }


  public override void _ParseEnd(GodotObject @object)
  {
    ParseGroups(@object);
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


            var vbox = groupControl.GetChild(0);
            if (orientation == ZExportGroupAttribute.Orientation.Horizontal)
            {
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

  private static Control GetInspectorSection(Node node)
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
}

#endif
