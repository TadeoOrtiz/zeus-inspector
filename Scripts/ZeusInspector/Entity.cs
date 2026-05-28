using Godot;
using Godot.Collections;
using ZeusInspector.Attributes;



[GlobalClass]
public partial class Entity : Resource
{

    [Export]
    [HorizontalGroup("Data", Title: "200")]
    [VerticalGroup("Data/Icons")]
    [PreviewField(120)]
    public CompressedTexture2D Icon;
    [Export]
    [VerticalGroup("Data/Icons")]
    [PreviewField(120)]
    public CompressedTexture2D IconSecundary;
    [Export]
    [VerticalGroup("Data/Right"), LabelRatio(.2f)]
    public int Health;
    [Export]
    [VerticalGroup("Data/Right"), LabelRatio(.2f)]
    public int Armor;
    [Export]
    [VerticalGroup("Data/Right"), LabelRatio(.2f)]
    public int Mana;
    [Export]
    [VerticalGroup("Data/Right"), LabelRatio(.2f)]
    public Array<int> MyArray;

    //[ExportToolButton("Testeo")]
    //[HorizontalGroup("General/Rigth"), LabelRatio(.2f)]
    //public Callable TestPiola => Callable.From<int>(Test);

    //[Export]
    //public TestStruct test;


    //public void Test(int cambiar)
    //{

    //}

}


