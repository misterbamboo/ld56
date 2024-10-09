using EnterTheMines.EnterTheMines.Events;
using Godot;

namespace EnterTheMines.EnterTheMines.PlayerCore.HUD;

public partial class TimerHUD : Control
{
    private Label label;

    private float startingTimeMins = 5;
    private float time;
    private string lastDisplay = "";
    private bool gameStarted = false;

    public override void _Ready()
    {
        label = GetNode<Label>("Label");
        GameEvents.Register<GameStartGameEvent>(StartTimer);
        GameEvents.Register<GameOverGameEvent>((GameOverGameEvent e) => { gameStarted = false; });
    }

    private void StartTimer(GameStartGameEvent e)
    {
        time = startingTimeMins * 60;
        gameStarted = true;
    }

    public override void _Process(double delta)
    {
        var deltaf = (float)delta;
        time = Mathf.Clamp(time - deltaf, 0, 999999999);
        DisplayTime();
        TriggerTimeout();
    }

    private void DisplayTime()
    {
        int mins = (int)(time / 60.0f);
        int timeInt = (int)time;
        int secs = timeInt % 60;

        string display = GetDisplay(mins, secs);
        if (display != lastDisplay)
        {
            lastDisplay = display;
            label.Text = display;
        }
    }

    private void TriggerTimeout()
    {
        if (gameStarted && time <= 0)
        {
            GameEvents.Raise(new EndDayGameEvent());
        }
    }

    private string GetDisplay(int mins, int secs)
    {
        string display = "";
        if (mins < 10)
        {
            display += "0";
        }
        display += mins.ToString();
        display += ":";

        if (secs < 10)
        {
            display += "0";
        }
        display += secs.ToString();

        return display;
    }
}