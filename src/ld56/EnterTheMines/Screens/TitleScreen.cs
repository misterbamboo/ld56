using EnterTheMines.EnterTheMines.Events;
using Godot;

namespace EnterTheMines.EnterTheMines.Screens;

public partial class TitleScreen : Control
{
    public void OnButtonPressed()
    {
        Visible = false;
        GameEvents.Raise(new GameStartGameEvent());
        GameEvents.Raise(new StartDayGameEvent());
    }
}
