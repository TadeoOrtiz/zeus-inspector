using Godot;
using Godot.Collections;
using ZeusInspector;
using ZeusInspector.Attributes;

public partial class MyNode : Node
{
  [Export]
  [ZExportGroup("Test Normal", "attrName", orientation: ZExportGroupAttribute.Orientation.Horizontal)]
  public int attrName_TESTINT;
  [Export]
  public int attrName_TESTINsT;

  [ZExportGroup()]
  [Export]
  public Vector2 Position;

  [Export]
  public TestStructSerializable TestStruct;

  [Export]
  public int a_TestGroup;

  [Export]
  private int number;

  public void Test()
  {

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
