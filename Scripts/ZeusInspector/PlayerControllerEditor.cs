using System;
using Godot;
using ZeusInspector.Editor;


[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public PlayerControllerEditor() : base()
    {
    }

    public override Control CreateInspectorGUI()
    {
        var container = new VBoxContainer();
        var label = new Label
        {
            Text = $"Wilson",
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        container.AddChild(label);

        var panel = new CenterContainer
        {
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
            SizeFlagsVertical = Control.SizeFlags.ExpandFill,
            Size = new Vector2(200, 200),
            MouseFilter = Control.MouseFilterEnum.Ignore
        };

        var textureRect = new TextureRect
        {
            Texture = GD.Load<Texture2D>("uid://o60g0ptjh0c8"),
            StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
            //ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            CustomMaximumSize = new Vector2(120, 120),
            //MouseFilter = Control.MouseFilterEnum.Ignore
        };
        textureRect.MouseEntered += () => label.Text = "¡Hola!";
        textureRect.MouseExited += () => label.Text = "Wilson";
        container.AddChild(panel);
        panel.AddChild(textureRect);

        return container;
    }




    public override EditorProperty CreatePropertyEditor(string name)
    {

        if (name == PlayerController.PropertyName.WalkSpeed || name == PlayerController.PropertyName.RunSpeed)
        {
            return new RandomIntEditor();
        }

        return base.CreatePropertyEditor(name);
    }
}
