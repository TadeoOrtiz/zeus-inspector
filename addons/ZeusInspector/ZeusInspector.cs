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

    private ZeusInspectorEditorPlguin _inspectorEditor;


    private readonly Dictionary<CustomDockAttribute, CustomDock> _inspectors = [];
    private readonly Dictionary<CustomDockAttribute, EditorDock> _docks = [];


    private Type _currentObjTypeName;
    private GodotObject _currentObjTarget;


    public override void _EnterTree()
    {
        _inspectorEditor = new();
        AddInspectorPlugin(_inspectorEditor);
        InitCustomDocks();
    }



    public override void _ExitTree()
    {
        RemoveCustomDocks();
        RemoveInspectorPlugin(_inspectorEditor);
    }

    

    public override bool _Handles(GodotObject @object)
    {
        foreach (var (attr, inspector) in _inspectors)
        {
            if (attr.EditorType == AttributeResolver.ResolveActualType(@object))
                return true;
        }
        _currentObjTypeName = null;
        _currentObjTarget = null;
        return false;
    }

    public override void _Edit(GodotObject @object)
    {
        if (@object == null) return;
        _currentObjTypeName = AttributeResolver.ResolveActualType(@object);
        _currentObjTarget = @object;
    }


    public override void _MakeVisible(bool visible)
    {
        if (visible)
        {
            foreach (var (attr, dock) in _docks)
            {
                if (attr.EditorType == _currentObjTypeName)
                {
                    foreach (var c in dock.GetChildren())
                        dock.RemoveChild(c);
                    dock.Open();
                    dock.MakeVisible();
                    _inspectors[attr].Target = _currentObjTarget;
                    var control = _inspectors[attr].CreateInspectorGUI();
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
            foreach (var (attr, dock) in _docks)
            {
                dock.Close();
                foreach (var c in dock.GetChildren())
                    dock.RemoveChild(c);
            }
        }
    }


    public void OnBeforeSerialize()
    {
        RemoveCustomDocks();
    }

    public void OnAfterDeserialize()
    {
        InitCustomDocks();
    }

    private void UpdateDocks()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var editorTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(CustomDock).IsAssignableFrom(t) && t != typeof(CustomDock));

        foreach (var type in editorTypes)
        {
            if (type == null) continue;
            var attr = type.GetCustomAttribute<CustomDockAttribute>();
            var editor = (CustomDock)Activator.CreateInstance(type);
            if (attr == null || editor == null) continue;
            _inspectors.Add(attr, editor);
        }
    }

    private void InitCustomDocks()
    {
        UpdateDocks();
        foreach (var (attr, inspector) in _inspectors)
        {
            var editorDock = new EditorDock
            {
                Title = attr.EditorType.Name,
                DefaultSlot = attr.DockSlot
            };
            _docks.Add(attr, editorDock);
            AddDock(editorDock);
            editorDock.Close();
        }
    }

    private void RemoveCustomDocks()
    {
        foreach (var dock in _docks.Values)
        {
            RemoveDock(dock);
        }
        _inspectors.Clear();
        foreach (var (attr, dock) in _docks)
        {
            dock.Close();
            foreach (var c in dock.GetChildren())
                dock.RemoveChild(c);
        }
        _docks.Clear();
    }

}
#endif
