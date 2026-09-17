#if TOOLS
using System.Collections.Generic;
using Godot;
using ZeusInspector.Attributes;

namespace ZeusInspector.Editor;

[Tool]
public partial class EditorParser : EditorInspectorPlugin
{

  private readonly Dictionary<string, List<InspectorAttribute>> customInspectors = [];
  public override bool _CanHandle(GodotObject @object)
  {
    customInspectors.Clear();
    return true;
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
#endif