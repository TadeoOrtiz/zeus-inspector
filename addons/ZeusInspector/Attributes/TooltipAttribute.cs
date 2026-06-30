using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class TooltipAttribute(string text) : InspectorAttribute
{

    private string tooltipText = text;
    private Node node;
    public override void ParseEditor(EditorProperty editor)
    {
        //editor.ChildOrderChanged += OnEditorChildEnteredTree;
        node = editor;
    }

    private void OnEditorChildEnteredTree()
    {
        foreach (var child in node.GetChildren())
        {
            if (child is PopupPanel popup)
            {
                foreach (var pchild in popup.GetChildren())
                {
                    if (pchild is VBoxContainer)
                    {
                        var textDescription = pchild.GetChild(1) as RichTextLabel;
                        textDescription.Text = tooltipText;

                    }
                }
            }
        }
    }

    //public override EditorProperty CreateEditor(Variant.Type type, string propName, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    //{
    //    return new TooltipEditorProperty(text);
    //}
}
