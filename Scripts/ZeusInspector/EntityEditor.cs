using Godot;
using ZeusInspector.Editor;

[CustomEditor(typeof(Entity))]
public class EntityEditor : Editor
{
    public EntityEditor() : base()
    {
        HiddenProperties.Add(Entity.PropertyName.Vida);
        HiddenProperties.Add(Entity.PropertyName.Equipo);
    }


    public override Control CreateInspectorGUI()
    {
        var container = new VBoxContainer();
        var label = new Label { Text = $"{Target.GetType().Name}:" };
        var textEdit = new LineEdit
        {
            Text = Target.Get<string>(Entity.PropertyName.Equipo),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };
        textEdit.TextChanged += OnEquipoChanged;


        var hb = new HBoxContainer();
        hb.AddChild(label);
        hb.AddChild(textEdit);
        container.AddChild(hb);
        var numeroSpin = new SpinBox
        {
            MinValue = 0,
            MaxValue = 100,
            Value = (int)Target.Get(Entity.PropertyName.Vida),
            SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
        };

        numeroSpin.ValueChanged += OnVidaChanged;
        container.AddChild(numeroSpin);

        return container;
    }

    private void OnVidaChanged(double value)
    {
        Target.Set(Entity.PropertyName.Vida, (int)value);
    }

    private void OnEquipoChanged(string value)
    {
        Target.Set(Entity.PropertyName.Equipo, value);
    }
}