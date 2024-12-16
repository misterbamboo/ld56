using EnterTheMines.EnterTheMines.Interactables.Items;
using Godot;

namespace EnterTheMines.EnterTheMines.PlayerCore;

public partial class RayCastHands : RayCast3D
{
	[Export]public float RayLength = 4f;

	private BaseItem hoverItem = null;
	private BaseItem pickedItem = null;
	private BaseItem lastHover = null;

	private bool lastMousePressed = false;

	private bool justHovered = false;
	private bool justUnhovered = false;

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
		if (other is not MPPickupHandle item) return;

		GD.Print($"{Multiplayer.GetUniqueId()} - {item.Name}");
		//item.AddChild(this);
        // UpdateSelectionDot();
		//HoldItem();
	}

	private void HoldItem()
	{
		var currentMousePressed = Input.IsActionPressed("action");
		if (currentMousePressed != lastMousePressed)
		{
			lastMousePressed = currentMousePressed;
			if(currentMousePressed)
			{
				TryPick();
			}
			else
			{
				ReleaseItem();
			}
		}
	}

	public void TryPick()
	{
		if (hoverItem is null) return;

		pickedItem = hoverItem;
		pickedItem.FreezeMe();
	}

	public void ReleaseItem()
	{
		if (pickedItem is null) return;

		pickedItem.UnfreezeMe();
		pickedItem = null;
	}
}
