using Godot;

namespace EnterTheMines.EnterTheMines.PlayerCore.HUD;

public partial class StaminaHUD : Control
{
    private ProgressBar progressBar;
    private Player player;
    private float opacity = 0.0f;

    public override void _Ready()
    {
        player = FindParent("Player") as Player;
        progressBar = GetNode<ProgressBar>("ProgressBar");
        Modulate = new Color(1, 1, 1, 0);
    }

    public override void _Process(double delta)
    {
        var deltaf = (float)delta;

        if (player.IsOutOfBreath)
        {
            opacity = Mathf.Sin(player.Stamina * 5) / 2 + 0.5f;
            Modulate = new Color(1, 1, 1, opacity);
        }
        else if (player.Stamina < player.TotalStaminaInSeconds)
        {
            if (opacity < 1)
            {
                opacity += deltaf;
                Modulate = new Color(1, 1, 1, opacity);
                if (opacity >= 1)
                {
                    opacity = 1;
                }
            }
        }
        else if(player.Stamina >= player.TotalStaminaInSeconds)
        {
            if (opacity > 0)
            {
                opacity -= deltaf;
                Modulate = new Color(1, 1, 1, opacity);
                if (opacity <= 0)
                {
                    opacity = 0;
                }
            }
        }

        progressBar.Value = player.Stamina / player.TotalStaminaInSeconds;

    }
}
