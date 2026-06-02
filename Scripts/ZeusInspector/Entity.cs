using Godot;
using Godot.Collections;
using ZeusInspector.Attributes;

[GlobalClass]
public partial class Entity : Resource
{

    [Export]
    //[HorizontalGroup("Data", Title: "Item")]
    //[VerticalGroup("Data/Icons")]
    //[PreviewField(120)]
    public Texture2D Icon;

    [Export]
    //[VerticalGroup("Data/Right")]
    public int Health;
    [Export]
    //[VerticalGroup("Data/Right")]
    public int Armor;
    [Export]
    //[VerticalGroup("Data/Right")]
    public int Mana;


}

