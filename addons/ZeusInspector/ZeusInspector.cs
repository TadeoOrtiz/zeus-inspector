#if TOOLS
using Godot;
using ZeusInspector.Editor;

namespace ZeusInspector;


[Tool]
public partial class ZeusInspector : EditorPlugin
{

    private InspectorEditor inspectorEditor;

    Control MainPanelInstance;

    public override void _EnterTree()
    {
        inspectorEditor = new InspectorEditor();

        AddInspectorPlugin(inspectorEditor);

        MainPanelInstance = new Control();
        // Add the main panel to the editor's main viewport.
        EditorInterface.Singleton.GetEditorMainScreen().AddChild(MainPanelInstance);
        // Hide the main panel. Very much required.
        _MakeVisible(false);
    }

    public override bool _HasMainScreen()
    {
        return true;
    }

    public override void _MakeVisible(bool visible)
    {
        if (MainPanelInstance != null)
        {
            MainPanelInstance.Visible = visible;
        }
    }

    public override string _GetPluginName()
    {
        return "Main Screen Plugin";
    }

    public override void _ExitTree()
    {
        RemoveInspectorPlugin(inspectorEditor);


        if (MainPanelInstance != null)
        {
            MainPanelInstance.QueueFree();
        }
    }

}
#endif
