using Godot;
using Godot.Collections;

[Tool]
public sealed partial class MiNodoEspecial : Node
{

    public struct MyStruct
    {
        public float Speed;
        public int Health;
        public string Label;
    }

    public override void _Ready()
    {
        GD.Print(_data.Health);
    }

    private MyStruct _data = new MyStruct { Speed = 10f, Health = 100, Label = "default" };

    public override Godot.Collections.Array<Godot.Collections.Dictionary> _GetPropertyList()
    {
        return
        [  
            // Grupo visual en el inspector  
            new() {
                { "name", "Data" },
                { "type", (int)Variant.Type.Nil },
                { "usage", (int)PropertyUsageFlags.Group },
                { "hint_string", "data_" },
            },
            new() {
                { "name", "data_speed" },
                { "type", (int)Variant.Type.Float },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", "0,1000,0.1" },
                { "usage", (int)PropertyUsageFlags.Default },
            },
            new() {
                { "name", "data_health" },
                { "type", (int)Variant.Type.Int },
                { "hint", (int)PropertyHint.Range },
                { "hint_string", "0,500,1" },
                { "usage", (int)PropertyUsageFlags.Default },
            },
            new() {
                { "name", "data_label" },
                { "type", (int)Variant.Type.String },
                { "usage", (int)PropertyUsageFlags.Default },
            },
        ];
    }

    public override Variant _Get(StringName property)
    {
        return property.ToString() switch
        {
            "data_speed" => _data.Speed,
            "data_health" => _data.Health,
            "data_label" => _data.Label,
            _ => default,
        };
    }

    public override bool _Set(StringName property, Variant value)
    {
        switch (property.ToString())
        {
            case "data_speed": _data.Speed = value.As<float>(); return true;
            case "data_health": _data.Health = value.As<int>(); return true;
            case "data_label": _data.Label = value.As<string>(); return true;
        }
        return false;
    }
}
