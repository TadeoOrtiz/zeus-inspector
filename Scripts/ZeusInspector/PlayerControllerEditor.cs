using System;
using Godot;
using ZeusInspector.Editor;


[CustomDock(typeof(PlayerController), EditorDock.DockSlot.Bottom)]
public class PlayerControllerEditor : CustomInspector
{
    public PlayerControllerEditor() : base()
    {
    }

    public override Control CreateInspectorGUI()
    {
        var container = new VBoxContainer();
        var label = new Label
        {
            Text = $"{((Node)Target).Name}",
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
        textureRect.MouseEntered += ChangeNumero;
        textureRect.MouseExited += ChangeNumero;
        container.AddChild(panel);
        panel.AddChild(textureRect);
        panel.AddChild(GD.Load<PackedScene>("uid://bdy8mysqmujnj").Instantiate());

        return container;
    }

    private void ChangeNumero()
    {
        Target.Set(PlayerController.PropertyName.WalkSpeed, (int)GD.Randi() % 100);
    }


}
