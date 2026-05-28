#if TOOLS
using System.Linq;
using System.Reflection;
using Godot;
using ZeusInspector.Editor;

namespace ZeusInspector;


[Tool]
public partial class ZeusInspector : EditorPlugin
{

    private ZeusInspectorEditorPlguin inspectorEditor;


    public override void _EnterTree()
    {
        inspectorEditor = new ZeusInspectorEditorPlguin();

        AddInspectorPlugin(inspectorEditor);

    }

    public override void _ExitTree()
    {
        RemoveInspectorPlugin(inspectorEditor);
    }

    private void UpdateEditorMap()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var editorTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(CustomInspector).IsAssignableFrom(t) && t != typeof(CustomInspector));

        foreach (var type in editorTypes)
        {
            var attr = type.GetCustomAttribute<CustomEditorAttribute>();
        }
    }

}
#endif
