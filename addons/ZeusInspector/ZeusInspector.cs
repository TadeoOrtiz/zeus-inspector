#if TOOLS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using ZeusInspector.Attributes;
using ZeusInspector.Editor;

namespace ZeusInspector;


[Tool]
public partial class ZeusInspector : EditorPlugin, ISerializationListener
{

    private ZeusInspectorEditorPlguin inspectorEditor;
    private AssetContextMenu assetContextMenu;


    private readonly Dictionary<CustomDockAttribute, CustomInspector> inspectors = [];
    private readonly Dictionary<CustomDockAttribute, EditorDock> docks = [];


    public override void _EnterTree()
    {
        inspectorEditor = new();
        assetContextMenu = new();

        AddInspectorPlugin(inspectorEditor);
        AddContextMenuPlugin(EditorContextMenuPlugin.ContextMenuSlot.FilesystemCreate, assetContextMenu);

        InitCustomInspectors();
    }



    public override void _ExitTree()
    {
        foreach (var dock in docks.Values)
        {
            RemoveDock(dock);
        }

        inspectors.Clear();
        docks.Clear();
        RemoveInspectorPlugin(inspectorEditor);
        RemoveContextMenuPlugin(assetContextMenu);
    }

    private void UpdateEditorMap()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var editorTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(CustomInspector).IsAssignableFrom(t) && t != typeof(CustomInspector));

        foreach (var type in editorTypes)
        {
            if (type == null) continue;
            var attr = type.GetCustomAttribute<CustomDockAttribute>();
            var editor = (CustomInspector)Activator.CreateInstance(type);
            if (attr == null || editor == null) continue;
            inspectors.Add(attr, editor);
        }
    }

    public override bool _Handles(GodotObject @object)
    {
        foreach (var (attr, inspector) in inspectors)
        {
            if (attr.EditorType == AttributeResolver.ResolveActualType(@object))
                return true;
        }
        currentObjTypeName = null;
        currentObjTarget = null;
        return false;
    }

    public override void _Edit(GodotObject @object)
    {
        if (@object == null) return;
        currentObjTypeName = AttributeResolver.ResolveActualType(@object);
        currentObjTarget = @object;
    }

    private Type currentObjTypeName;
    private GodotObject currentObjTarget;



    public override void _MakeVisible(bool visible)
    {
        if (visible)
        {
            foreach (var (attr, dock) in docks)
            {
                if (attr.EditorType == currentObjTypeName)
                {
                    foreach (var c in dock.GetChildren())
                        dock.RemoveChild(c);
                    dock.Open();
                    dock.MakeVisible();
                    inspectors[attr].Target = currentObjTarget;
                    var control = inspectors[attr].CreateInspectorGUI();
                    dock.AddChild(control);
                }
                else
                {
                    dock.Close();
                    foreach (var c in dock.GetChildren())
                        dock.RemoveChild(c);
                }
            }
        }
        else
        {
            foreach (var (attr, dock) in docks)
            {
                dock.Close();
                foreach (var c in dock.GetChildren())
                    dock.RemoveChild(c);
            }
        }
    }


    public void OnBeforeSerialize()
    {
        foreach (var dock in docks.Values)
        {
            RemoveDock(dock);
        }
        inspectors.Clear();
        foreach (var (attr, dock) in docks)
        {
            dock.Close();
            foreach (var c in dock.GetChildren())
                dock.RemoveChild(c);
        }
        docks.Clear();
    }

    public void OnAfterDeserialize()
    {
        InitCustomInspectors();
    }

    private void InitCustomInspectors()
    {
        UpdateEditorMap();
        foreach (var (attr, _) in inspectors)
        {
            var editorDock = new EditorDock
            {
                Title = attr.EditorType.Name,
                DefaultSlot = attr.DockSlot
            };
            docks.Add(attr, editorDock);
            AddDock(editorDock);
            editorDock.Close();
        }
    }

}
#endif
