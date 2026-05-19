using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace ZeusInspector.Editor;

[Tool]
public partial class InspectorEditor : EditorInspectorPlugin
{
    private Dictionary<Type, Type> _editorMap = new();
    private Stack<Editor> _editorStack = new();

    public InspectorEditor()
    {
        _UpdateEditorMap();
    }

    private void _UpdateEditorMap()
    {
        _editorMap.Clear();
        var assembly = Assembly.GetExecutingAssembly();
        var editorTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(Editor).IsAssignableFrom(t) && t != typeof(Editor));

        foreach (var type in editorTypes)
        {
            var attr = type.GetCustomAttribute<CustomEditorAttribute>();
            if (attr != null)
            {
                _editorMap[attr.EditorType] = type;
            }
        }
    }

    private Type _GetTargetType(GodotObject @object)
    {
        if (@object == null) return null;

        var type = @object.GetType();
        if (_editorMap.ContainsKey(type)) return type;

        var script = @object.GetScript().As<Script>();
        if (script != null)
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(script.ResourcePath);
            return _editorMap.Keys.FirstOrDefault(t => t.Name == className);
        }

        return null;
    }

    public override bool _CanHandle(GodotObject @object)
    {
        return _GetTargetType(@object) != null;
    }

    public override void _ParseBegin(GodotObject @object)
    {
        var targetType = _GetTargetType(@object);
        if (targetType != null && _editorMap.TryGetValue(targetType, out Type editorType))
        {
            try
            {
                var editor = (Editor)Activator.CreateInstance(editorType);
                editor.Target = @object;
                _editorStack.Push(editor);
            }
            catch (Exception e)
            {
                GD.PrintErr($"Error al crear el editor {editorType.Name}: {e.Message}");
            }
        }
    }

    public override void _ParseCategory(GodotObject @object, string category)
    {
        if (_editorStack.Count > 0)
        {
            var currentEditor = _editorStack.Peek();
            if (currentEditor.Target == @object && !currentEditor.WasGuiAdded)
            {
                var gui = currentEditor.CreateInspectorGUI();
                if (gui != null)
                {
                    AddCustomControl(gui);
                    currentEditor.WasGuiAdded = true;
                }
            }
        }
    }

    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        if (_editorStack.Count > 0)
        {
            var currentEditor = _editorStack.Peek();
            
            if (currentEditor.Target == @object)
            {
                if (currentEditor.ShouldHideProperty(name))
                {
                    return true;
                }
                
                if (currentEditor.CreatePropertyEditor(name) is EditorProperty customEditor)
                {
                    AddPropertyEditor(name, customEditor);
                    return true;
                }
            }
        }

        return false;
    }

    public override void _ParseEnd(GodotObject @object)
    {
        if (_editorStack.Count > 0 && _editorStack.Peek().Target == @object)
        {
            _editorStack.Pop();
        }
    }
}
