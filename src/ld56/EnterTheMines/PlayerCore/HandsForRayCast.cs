using EnterTheMines.EnterTheMines.Interactables.Items;
using EnterTheMines.EnterTheMines.Services;
using Godot;
using System.Linq;

namespace EnterTheMines.EnterTheMines.PlayerCore;

public partial class HandsForRayCast : Node3D
{
    public const float RAY_LENGTH = 1000f;

    private float pickDistance = 3f;

    private GameManager gameManager;
    private Camera3D cam;

    private BaseItem hoverItem = null;
    private BaseItem pickedItem = null;
    private BaseItem lastHover = null;

    private bool lastMousePressed = false;

    private bool justHovered = false;
    private bool justUnhovered = false;

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>(GameManager.Path);
        cam = GetParent<Player>().GetNode<Camera3D>("Neck/Camera3D");
    }

    public override void _Process(double delta)
    {
        UpdateSelectionDot();
        HoldItem();
        MovePickedItem();
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

    private void UpdateSelectionDot()
    {
        if (justHovered)
        {
            gameManager.SetAction("Hold to grab");
        }
        if(justUnhovered)
        {
            gameManager.SetAction("");
        }
    }

    private void MovePickedItem()
    {
        if (pickedItem is null) return;
        if (pickedItem.Grinding) return;

        var forward = cam.GlobalTransform.Basis.Z.Normalized() * 2;
        var newPos = GlobalPosition - forward;

        var targetFloorPos = new Vector3(newPos.X, 0, newPos.Z);
        var handsFloorPos = new Vector3(GlobalPosition.X, 0, GlobalPosition.Z);

        var directionVector = (targetFloorPos - handsFloorPos).Normalized();
        var targetPos = handsFloorPos + directionVector * new Vector3(0, newPos.Y, 0);

        var yaw = Mathf.Atan2(directionVector.X, directionVector.Z);

        pickedItem.Rb.GlobalPosition = targetPos;
        pickedItem.Rb.Rotation = new Vector3(pickedItem.Rb.GlobalRotation.X, yaw, pickedItem.Rb.GlobalRotation.Z);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (pickedItem is not null) return;
        lastHover = hoverItem;

        BaseItem newHoverItem = null;

        var spaceState = GetWorld3D().DirectSpaceState;
        var mousePos = GetViewport().GetMousePosition();

        var origin = cam.ProjectRayOrigin(mousePos);
        var end = origin + cam.ProjectRayNormal(mousePos) * RAY_LENGTH;
        var query = PhysicsRayQueryParameters3D.Create(origin, end);
        query.CollisionMask = 4; // bitmask
        query.CollideWithAreas = true;

        var result = spaceState.IntersectRay(query);
        if (result.Any())
        {
            var collider = result["collider"].As<Node3D>();
            var distanceVector = result["position"].AsVector3() - GlobalPosition;
            var distance = distanceVector.Length();
            if(distance < pickDistance)
            {
                var parent = collider.GetParent();
                if(parent is BaseItem item)
                {
                    newHoverItem = item;
                }
            }
        }

        hoverItem = newHoverItem;

        justHovered = lastHover is null && hoverItem is not null;
        justUnhovered = lastHover is not null && hoverItem is null;
    }

    public void TryPick()
    {
        if (hoverItem is null) return;

        pickedItem = hoverItem;
        pickedItem.Freeze();
    }

    public void ReleaseItem()
    {
        if (pickedItem is null) return;

        pickedItem.Unfreeze();
        pickedItem = null;
    }
}
