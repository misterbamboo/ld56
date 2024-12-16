#if TOOLS
using Godot;
using System;

[Tool]
public partial class EnterTheMinesPlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		var script = GD.Load<Script>("res://EnterTheMines/Interactables/Items/MPPickupHandle.cs");
		var icon = GD.Load<Texture2D>("res://EnterTheMines/Interactables/Items/MPPickupHandle.svg");
        AddCustomType("MPPickupHandle", "Enter The Mines", script, icon);
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
	}
}
#endif
