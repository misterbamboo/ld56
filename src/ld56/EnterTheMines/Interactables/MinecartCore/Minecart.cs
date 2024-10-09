using EnterTheMines.EnterTheMines.PlayerCore;
using EnterTheMines.EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.Interactables.MinecartCore;

public partial class Minecart : PathFollow3D
{
    private GameManager gameManager;
    private Area3D pushCollider;

    private bool playerInBox = false;
    private Player playerRef;

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>(GameManager.Path);
        pushCollider = GetNode<Area3D>("PushCollider");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("action"))
        {
            if (playerRef != null && !playerRef.IsAttachedToCart)
            {
                playerRef.AttachToCart();
                playerRef.Reparent(this);
            }
        }

        if (@event.IsActionReleased("action"))
        {
            if (playerRef != null && playerRef.IsAttachedToCart)
            {
                playerRef.DetachFromCart();
                playerRef.Reparent(FindParent("Master"));
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (playerRef == null || !playerRef.IsAttachedToCart)
        {
            return;
        }

        var deltaf = (float)delta;

        float direction = Input.GetAxis("back", "forward");

        float speed = Player.SPEED * Player.CART_SPEED_MOD;

        if (playerRef.IsOutOfBreath)
        {
            playerRef.Stamina += deltaf * playerRef.StaminaRechargeRateMultiplier;
            if (playerRef.Stamina > playerRef.TotalStaminaInSeconds * playerRef.OutOfBreathMinimumRefillPercent)
            {
                playerRef.IsOutOfBreath = false;
            }
        }
        else if (Input.IsActionPressed("run"))
        {
            if (playerRef.Stamina > 0)
            {
                speed *= playerRef.RunSpeedModifier;
                playerRef.Stamina -= deltaf;
                if (playerRef.Stamina <= 0)
                {
                    playerRef.IsOutOfBreath = true;
                    playerRef.PlayOutOfBreathSound();
                }
            }
        }
        else if (playerRef.Stamina < playerRef.TotalStaminaInSeconds)
        {
            playerRef.Stamina += deltaf * playerRef.StaminaRechargeRateMultiplier;
        }

        if (Input.IsActionPressed("run"))
        {
            speed *= playerRef.RunSpeedModifier;
        }

        float newProgress = Progress + direction * speed;
        Progress = Mathf.Clamp(newProgress, 0, (GetParent() as Path3D).Curve.GetBakedLength());
    }

    public void OnBodyEntered(Node3D body)
    {
        gameManager.SetAction("Hold to push/pull");
        playerInBox = true;
        playerRef = (Player)body;
    }

    public void OnBodyExited(Node3D body)
    {
        gameManager.SetAction("");
        playerInBox = false;
        playerRef = null;
    }
}
