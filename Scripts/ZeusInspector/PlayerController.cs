using Godot;
using ZeusInspector.Attributes;


[GlobalClass]
[CreateAssetMenu("Data/Entity")]
public partial class PlayerController : CharacterBody3D
{
    public static PlayerController Instance { get; private set; }

    [Export] public Label DebugLabel;
    [Export] public Entity PlayerEntity;

    [ExportGroup("Stats")]
    [ExportSubgroup("Movement")]
    [Export]
    public int numero;



    [ExportGroup("Movement")]
    [Export] public float WalkSpeed = 10f;
    [Export] public float RunSpeed = 20f;
    [Export] public float ClimbSpeed = 8f;
    [Export] public float DashSpeed = 16f;
    [Export] public float DashDuration = 1f;
    [Export] public float JumpHeight = 2f;
    [Export] public float JumpDistance = 4f;
    [Export] public float ClimbWallForce = 20f;
    [ExportGroup("Attacks")]
    [Export] public float TopBarrelSpeed = 10f;
    [Export] public float TimeToReachTopBarrelSpeed = 1.5f;
    [Export] public float CrusherMaxChargeTime = 2f;
    [Export] public float CrusherMinChargeTime = 0.2f;

    [ExportGroup("Components")]
    [Export]
    private Node bodyAnimationComponent;
    [Export]
    public Node3D triggerPos;
    [Export]
    public float triggerOffset;

    public float MoveSpeed { get; set; }
    public float Gravity { get; private set; }
    public float JumpVelocity { get; private set; }


}
