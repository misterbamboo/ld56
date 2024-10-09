using Godot;

namespace EnterTheMines.EnterTheMines.PlayerCore.HUD;

public partial class CenterDotHUD : Label
{
    public override void _UnhandledInput(InputEvent e)
    {
        if (e.IsActionPressed("action"))
        {
            Visible = false;
        }
        else if (e.IsActionReleased("action"))
        { 
            Visible = true;
        }
    }
}
