using Godot;
using ZeusInspector.Editor;

namespace ZeusInspector.Demo;

[CustomDock(typeof(Item))]
public class ItemCustomDock : CustomDock
{
    public override Control CreateInspectorGUI()
    {
        var control = new Control();
        control.CustomMinimumSize = new(0, 200);

        string text = $"The {Target.Get(Item.PropertyName.name)} has {Target.Get(Item.PropertyName.Damage)} damage and {Target.Get(Item.PropertyName.Durability)} durability.";

        var label = new Label
        {
            Text = text
        };

        control.AddChild(label);
        return control;
    }
}
