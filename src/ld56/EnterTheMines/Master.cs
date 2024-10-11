using EnterTheMines.EnterTheMines.Events;
using Godot;

public partial class Master : Node
{
	public override void _Ready()
	{
		GameEvents.Raise(new GameLaunchedGameEvent());
	}
}
