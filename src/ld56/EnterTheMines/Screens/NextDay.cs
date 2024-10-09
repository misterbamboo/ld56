using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.Screens;

public partial class NextDay : Control
{
    private GameManager gameManager;
    private Label label;
    private Label daysLeftLabel;
    private Label uncashedProfitsLabel;


    public override void _Ready()
    {
        gameManager = GetNode<GameManager>(GameManager.Path);
        label = GetNode<Label>("Label");
        daysLeftLabel = GetNode<Label>("DaysLeftLabel");
        uncashedProfitsLabel = GetNode<Label>("UncashedProfitsLabel");

        GameEvents.Register<EndDayGameEvent>((_)=>AppearScreen());
        GameEvents.Register<GameOverGameEvent>((_)=>HideScreen());
        HideScreen();
    }

    public void AppearScreen()
    {
        if (!gameManager.PlayerAlive)
        {
            label.Text = "You died in the mines, all uncashed profits lost.";
        }
        else if (gameManager.Player.IsInside)
        {
            label.Text = "The truck left without you,\nall uncashed profits lost.";
        }
        else
        {
            label.Text = "Another day, another trip to the mines";
            uncashedProfitsLabel.Text = $"{gameManager.UncashedInMoney} $";
        }

        daysLeftLabel.Text = $"{gameManager.WeekDuration - gameManager.CurrentDay}";
        Visible = true;
    }

    private void _OnButtonPressed()
    {
        gameManager.NextDay();
    }

    public void HideScreen()
    {
        Visible = false;
    }
}
