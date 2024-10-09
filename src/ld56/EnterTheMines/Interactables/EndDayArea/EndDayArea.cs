using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.Interactables;

public partial class EndDayArea : Area3D
{
    private GameManager gameManager;
    private bool playerIn = false;

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>(GameManager.Path);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("action"))
        {
            if (playerIn)
            {
                GameEvents.Raise(new EndDayGameEvent());
                GetViewport().SetInputAsHandled();
            }
        }
    }

    public void OnBodyEntered(Node3D body)
    {
        playerIn = true;
        gameManager.SetAction("Leave");
    }

    public void OnBodyExited(Node3D body)
    {
        playerIn = false;
        gameManager.SetAction("");
    }
}
