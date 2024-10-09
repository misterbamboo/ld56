using EnterTheMines.EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.Interactables.Items;

public partial class BaseItem : Node3D
{
    private GameManager gameManager;
    [Export]public int Price { get; private set; }
    public RigidBody3D Rb { get; private set; }
    public bool Grinding { get; private set; }

    public override void _Ready()
	{
        gameManager = GetNode<GameManager>(GameManager.Path);
        Rb = GetNode<RigidBody3D>("RigidBody3D");
        Rb.FreezeMode = RigidBody3D.FreezeModeEnum.Kinematic;
    }

	public void Freeze()
    {
        Rb.Rotation = Vector3.Zero;
        Rb.Freeze = true;
    }

    public void Unfreeze()
    {
        Rb.Freeze = false;
    }

    public void Grind()
    {
        Grinding = true;
    }

    public void Destroy()
    {
        gameManager.GiveMoney(Price);
        QueueFree();
    }
}
