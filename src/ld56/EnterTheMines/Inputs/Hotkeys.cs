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
		if (keyEvent.Keycode == Key.Escape)
		{
			GameEvents.Raise(new ExitRequestedGameEvent());
		}
	}
}
