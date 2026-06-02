using System;
using Godot;
using Godot.Collections;

namespace ZeusInspector.Attributes;

public enum GroupOrientation
{
    Horizontal,
    Vertical
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class GroupAttribute(string groupName, GroupOrientation orientation = GroupOrientation.Horizontal, string title = "") : InspectorAttribute
{
    public string GroupName { get; } = groupName;
    public GroupOrientation Orientation { get; } = orientation;
    public string Title { get; } = title;

    public static void Cleanup(Node root)
    {
        var toRemove = new Godot.Collections.Array<Node>();
        foreach (var child in root.GetChildren())
        {
            if (child.HasMeta("__zeus_group"))
                toRemove.Add(child);
            else
                Cleanup(child);
        }
        foreach (var child in toRemove)
        {
            root.RemoveChild(child);
            child.QueueFree();
        }
    }

    public override void ParseEditor(EditorProperty editor)
    {
        Container currentParent = FindRootContainer(editor);
        var segments = GroupName.Split('/');
        var editorIndex = editor.GetIndex();

        for (int i = 0; i < segments.Length; i++)
        {
            string segment = segments[i];
            bool isFirst = i == 0;
            //GD.Print(segment);

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
            marco.Set("theme_override_constants/separation", 0);
            marco.SetMeta("__zeus_group", true);

            if (!string.IsNullOrEmpty(Title))
            {
                var label = new Label
                {
                    Text = Title,
                    Name = $"Label/{segment}",
                    SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter,
                };
                marco.AddChild(label);
            }

            Container contentContainer = Orientation == GroupOrientation.Vertical
                ? new VBoxContainer()
                : new HBoxContainer();
            contentContainer.Name = $"{segment}__c";


            if (Orientation == GroupOrientation.Horizontal)
            {
                contentContainer.GrowHorizontal = Control.GrowDirection.Begin;
                contentContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                contentContainer.SizeFlagsVertical = Control.SizeFlags.ShrinkBegin;


                editor.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
                editor.SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;
            }
            else
            {
                contentContainer.GrowVertical = Control.GrowDirection.Begin;
                contentContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
                contentContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

                editor.SizeFlagsVertical = Control.SizeFlags.ShrinkBegin;
                editor.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

            }


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

        editor.Reparent(currentParent);
    }

    private static Container FindRootContainer(EditorProperty editor)
    {
        Container current = editor.GetParent() as Container;
        while (current != null)
        {
            var name = current.Name.ToString();
            if (name.EndsWith("__c"))
            {
                current = current.GetParent() as Container;
                continue;
            }

            if (current is VBoxContainer marco && GetContentContainer(marco) != null)
            {
                current = current.GetParent() as Container;
                continue;
            }

            break;
        }
        return current ?? editor.GetParent() as Container;
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
