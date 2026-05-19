using Godot;



[GlobalClass]
public partial class Entity : Resource
{

    [Export]
    public int Vida;
    [Export]
    public string Equipo;
    [Export]
    public Curve VidaCurve;

}
