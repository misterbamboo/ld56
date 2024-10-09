using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.PlayerCore.HUD;

public partial class MoneyHUD : Control
{
    private GameManager gameManager;
    private Label label;
    private Label label2;
    private Label label3;
    private AudioStreamPlayer2D audio;
    private Timer displayTimer;

    private float opacity = 0.0f;
    private bool shouldHide = true;
    private float previousMoney = 0f;

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>(GameManager.Path);
        label = GetNode<Label>("Label");
        label2 = GetNode<Label>("QuotaDisplay/Label2");
        label3 = GetNode<Label>("QuotaDisplay/Label3");
        audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        displayTimer = GetNode<Timer>("Timer");


        GameEvents.Register<MoneyReceivedGameEvent>(OnMoneyReceived);
        GameEvents.Register<GameStartGameEvent>(OnGameStart);

        Modulate = new Color(1, 1, 1, 0);
    }

    public void OnMoneyReceived(MoneyReceivedGameEvent e)
    {
        ReceiveMoney();
    }

    public void OnGameStart(GameStartGameEvent e)
    {
        ReceiveMoney();
    }

    public override void _Process(double delta)
    {
        var deltaf = (float)delta;

        if (shouldHide)
        {
            if (opacity > 0)
            {
                opacity -= deltaf;
                if (opacity <= 0) opacity = 0;
            }
        }
        else
        {
            if (opacity < 1)
            {
                opacity += deltaf;
                if (opacity >= 1) opacity = 1;
            }
        }

        Modulate = new Color(1, 1, 1, opacity);
    }

    public void ReceiveMoney()
    {
        var currentMoney = gameManager.UncashedInMoney;
        label.Text = $"{gameManager.Money} $";
        label2.Text = $"{gameManager.UncashedInMoney} $";
        label3.Text = $"{gameManager.Quota} $";

        if (currentMoney > previousMoney)
        {
            audio.Play(0);
        }

        previousMoney = currentMoney;
        opacity = 1;
        shouldHide = false;
        displayTimer.Start();
    }

    public void DisplayTimeout()
    {
        shouldHide = true;
    }
}
