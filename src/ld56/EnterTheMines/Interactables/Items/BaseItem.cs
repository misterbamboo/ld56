using EnterTheMines.EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.Interactables.Items;

public partial class BaseItem : RigidBody3D
{
    private GameManager gameManager;
    [Export]public int Price { get; private set; }
    public bool Grinding { get; private set; }

    public override void _Ready()
	{
        gameManager = GetNode<GameManager>(GameManager.Path);
        FreezeMode = RigidBody3D.FreezeModeEnum.Kinematic;
    }

	public void FreezeMe()
    {
        Rotation = Vector3.Zero;
        Freeze = true;
    }

    public void UnfreezeMe()
    {
        Freeze = false;
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
