using Godot;

namespace ZeusInspector.Attributes;

public partial class TooltipEditorProperty : EditorProperty
{

    private string tooltipText;

    public TooltipEditorProperty()
    {
    }

    public TooltipEditorProperty(string text)
    {
        tooltipText = text;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationChildOrderChanged)
        {
            foreach (var child in GetChildren())
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
    }



}