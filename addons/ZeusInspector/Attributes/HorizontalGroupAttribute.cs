using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class HorizontalGroupAttribute(string groupName) : InspectorAttribute
{
    public string GroupName { get; } = groupName;

    private static readonly HashSet<(GodotObject, string)> _seenGroups = new();

    public override void Apply(EditorProperty editor) { }

    public override AttributeResult Apply(EditorProperty editor, string name, GodotObject obj, EditorInspectorPlugin inspector)
    {
        var key = (obj, GroupName);
        bool isFirst = !_seenGroups.Contains(key);

        if (isFirst)
            _seenGroups.Add(key);

        inspector.AddPropertyEditor(
            name, editor,
            !isFirst,
            name
        );

        return AttributeResult.Handled;
    }

    public override void OnParseEnd(GodotObject obj)
    {
        var keys = _seenGroups.Where(k => k.Item1 == obj).ToList();
        foreach (var key in keys)
            _seenGroups.Remove(key);
    }
}
