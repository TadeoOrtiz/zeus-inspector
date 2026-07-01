using Godot;
using ZeusInspector.Demo;
using ZeusInspector.Editor;

[CustomDock(typeof(Item))]
public class ItemCustomDock : CustomDock
{
    public override Control CreateInspectorGUI()
    {
        var control = new Control();

        var label = new Label
        {
            Text = (string)Target.Get(Item.PropertyName.name)
        };

        control.AddChild(label);
        return control;
    }
}