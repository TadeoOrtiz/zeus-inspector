using Godot;
using Godot.Collections;

public partial class MyNode : Node
{
    [Export]
    public int TEstM;

    [Export]
    public TestStructSerializable TestStruct = new();


    public override void _Ready()
    {
        base._Ready();
        GD.Print(TestStruct.Nombre);
    }

}


[System.Serializable]
public struct TestStructSerializable
{
    public int X = 10;
    public string Nombre;

    public TestStructSerializable()
    {

    }

    // Struct → Dictionary (para serializar)
    public Godot.Collections.Dictionary ToGodotDictionary()
    {
        return new Godot.Collections.Dictionary
        {
            { "_type", typeof(TestStructSerializable).AssemblyQualifiedName},
            { "x", X },
            { "nombre", Nombre }
        };
    }

    // Dictionary → Struct (para deserializar)
    public static TestStructSerializable FromGodotDictionary(Godot.Collections.Dictionary dict)
    {
        return new TestStructSerializable
        {
            X = dict["x"].AsInt32(),
            Nombre = dict["nombre"].AsString()
        };
    }

}
