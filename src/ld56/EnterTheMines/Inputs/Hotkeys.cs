using EnterTheMines.EnterTheMines.Events;
using Godot;

public partial class Hotkeys : Node
{
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent)
		{
			HandleEscapeKey(keyEvent);
		}
	}

	private static void HandleEscapeKey(InputEventKey keyEvent)
	{
		if (keyEvent.IsActionReleased("exit"))
		{
			GameEvents.Raise(new ExitRequestedGameEvent());
		}
	}
}
