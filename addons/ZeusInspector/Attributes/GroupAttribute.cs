using System;
using Godot;

namespace ZeusInspector.Attributes;

public enum GroupOrientation
{
    Horizontal,
    Vertical
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class GroupAttribute(string groupName, GroupOrientation orientation = GroupOrientation.Horizontal, bool showTitle = false) : InspectorAttribute
{
    public string GroupName { get; } = groupName;
    public GroupOrientation Orientation { get; } = orientation;
    public bool ShowTitle { get; } = showTitle;

    public override void Apply(EditorProperty editor)
    {
        var segments = GroupName.Split('/');
        Container currentParent = editor.GetParent() as Container;
        var editorIndex = editor.GetIndex();

        for (int i = 0; i < segments.Length; i++)
        {
            string segment = segments[i];
            bool isFirst = i == 0;

            VBoxContainer marco = FindMarco(currentParent, editor, segment);

            if (marco != null)
            {
                currentParent = GetContentContainer(marco) ?? marco;
                continue;
            }

            marco = new VBoxContainer
            {
                Name = segment,
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            };
            marco.Set("theme_override_constants/separation", 10);

            if (showTitle)
            {
                var label = new Label
                {
                    Text = segment,
                    SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter,
                };
                marco.AddChild(label);
            }

            Container contentContainer = orientation == GroupOrientation.Vertical
                ? new VBoxContainer()
                : new HBoxContainer();
            contentContainer.Name = $"{segment}__c";
            contentContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            marco.AddChild(contentContainer);

            if (isFirst)
            {
                editor.AddSibling(marco);
                marco.GetParent().MoveChild(marco, editorIndex);
            }
            else
            {
                currentParent.AddChild(marco);
            }

            currentParent = contentContainer;
        }

        editor.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        editor.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        editor.Reparent(currentParent);
    }

    private static VBoxContainer FindMarco(Container parent, Node exclude, string name)
    {
        foreach (var child in parent.GetChildren())
        {
            if (child != exclude && child.Name == name && child is VBoxContainer marco)
                return marco;
        }
        return null;
    }

    private static Container GetContentContainer(VBoxContainer marco)
    {
        var contentName = $"{marco.Name}__c";
        foreach (var child in marco.GetChildren())
        {
            if (child.Name == contentName && child is Container c)
                return c;
        }
        return null;
    }
}
