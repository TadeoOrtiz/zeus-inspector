using Godot;
using ZeusInspector.Attributes;



[GlobalClass]
public partial class Entity : Resource
{

    [Export]
    [HorizontalGroup("General", showTitle: true), LabelRatio(.2f)]
    [PreviewField(64)]
    public Texture2D Name;
    [Export]
    [VerticalGroup("General/Left"), LabelRatio(.2f)]
    public int Health;
    [Export]
    [VerticalGroup("General/Left"), LabelRatio(.2f)]
    public int Armor;
    [Export]
    [VerticalGroup("General/Left"), LabelRatio(.2f)]
    public int Mana;

}
