using Godot;
using ZeusInspector.Attributes;

namespace ZeusInspector.Demo;

public partial class Item : Resource
{
    [Export]
    public string name;

    [HorizontalGroup("Data", "Data")]
    [Export]
    public int Damage;
    [HorizontalGroup("Data")]
    [Export]
    public int Durability;
    [Export]
    public int Cost;
}