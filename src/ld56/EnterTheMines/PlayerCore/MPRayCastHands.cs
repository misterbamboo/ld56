using EnterTheMines.EnterTheMines.Interactables.Items;
using Godot;

namespace EnterTheMines.EnterTheMines.PlayerCore;

public partial class MPRayCastHands : RayCast3D
{
	[Export]public float RayLength = 4f;
    private GodotObject _currentlyHoveredObject = null;

    public override void _Ready()
	{
        SetMultiplayerAuthority(GetParent().GetParent<MPPlayer>().PlayerId);

        if (GetMultiplayerAuthority() != Multiplayer.GetUniqueId())
        {
            SetProcess(false);
            SetProcessInput(false);
            SetProcessUnhandledInput(false);
        }

        TargetPosition = new Vector3(0, 0, -RayLength);
    }

    public override void _Process(double delta)
    {
        var other = GetCollider();

        if (_currentlyHoveredObject == other) return;

        if (_currentlyHoveredObject != null)
        {
            JustUnhovered(_currentlyHoveredObject);
        }

        if (other != null)
        {
            JustHovered(other);
        }

        _currentlyHoveredObject = other;
    }

    public void JustHovered(GodotObject obj)
    {
        if (obj is MPPickupHandle handle)
        {
            var item = handle.GetItem().ItemProperties;
            GD.Print($"{Multiplayer.GetUniqueId()} - JustHovered - {item.ItemName}");
        }
    }

    public void JustUnhovered(GodotObject obj)
    {
        if (obj is MPPickupHandle handle)
        {
            var item = handle.GetItem().ItemProperties;
            GD.Print($"{Multiplayer.GetUniqueId()} - JustUnHovered - {item.ItemName}");
        }
    }
}
