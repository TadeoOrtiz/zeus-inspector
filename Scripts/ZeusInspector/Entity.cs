using Godot;
using Godot.Collections;
using ZeusInspector.Attributes;

[GlobalClass]
[Icon("res://Scripts/node/troll.svg")]
public partial class Entity : Resource
{

    [Export]
    [HorizontalGroup("Data", Title: "Item")]
    [VerticalGroup("Data/Icons")]
    [Tooltip("This is the icon of the entity. JKLASDKJLKASLJKDJKLASDLKJA")]
    [PreviewField(120)]
    public Texture2D Icon;

    [Export]
    [VerticalGroup("Data/Right")]
    public int Health;
    [Export]
    [VerticalGroup("Data/Right")]
    public int Armor;
    [Export]
    [VerticalGroup("Data/Right")]
    public int Mana;
    [HorizontalGroup("Enums")]
    [Export]
    public int Rarity;
    [HorizontalGroup("Enums")]
    [Export]
    public int Slot;
    [HorizontalGroup("Enums")]
    [Export]
    public int ASD;



}

