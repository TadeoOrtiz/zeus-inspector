#if TOOLS
using Godot;

namespace ZeusInspector.Editor;

[Tool]
public partial class TestInspectorPlugin : EditorInspectorPlugin
{

    public override bool _CanHandle(GodotObject @object)
    {
        return true;
    }

    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string propName, PropertyHint hintType,
    string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        return false;
    }
}

#endif
