using EnterTheMines.EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.PlayerCore;

public partial class MPPlayer : CharacterBody3D
{
    public const float SPEED = 4.0f;
    public const float JUMP_VELOCITY = 4.5f;
    public const float CART_SPEED_MOD = 0.008f;
    public const float TIME_TO_SCARE_SECONDS = 0.5f;

    private Camera3D camera;

    private float sensitivity = 0.001f;
    public float RunSpeedModifier { get; private set; } = 1.5f;
    private GameManager gameManager;
    private Vector2 CameraRotation = new Vector2(90, 0);
    public float TotalStaminaInSeconds { get; private set; } = 5.0f;
    public float Stamina { get; set; } = 5.0f;
    public float StaminaRechargeRateMultiplier { get; private set; } = 1.2f;
    public float OutOfBreathMinimumRefillPercent { get; private set; } = 0.75f;
    public bool IsOutOfBreath { get; set; } = false;

    public bool IsMoving { get; private set; } = false;

    public const float JumpVelocity = 4.5f;

    public int wakeupFrames = 1;

    public override void _EnterTree()
    {
        SetMultiplayerAuthority(int.Parse(Name));
        Rpc(MethodName.Test);
    }

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>(GameManager.Path);
        camera = GetNode<Camera3D>("Camera3D");

        if (!IsMultiplayerAuthority()) return;
        
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
        GD.PrintRich($"[color=green] MPPlayer {Name} Ready![/color]");
        camera.Current = true;
    }

    [Rpc]
    public void Test()
    {

    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (!IsMultiplayerAuthority()) return;

        if (e is InputEventMouseMotion mouseMotion)
        {
            RotateY(-mouseMotion.Relative.X * sensitivity);
            camera.RotateX(Mathf.Clamp(-mouseMotion.Relative.Y * sensitivity, -Mathf.Pi/2, Mathf.Pi/2));
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsMultiplayerAuthority()) return;

        // for some reason le jeu brise si on attend pas un frame avant de bouger
        // le joueur se fait fling a l'autre bout de la map
        if (wakeupFrames > 0)
        {
            wakeupFrames--;
            return;
        }

        float deltaf = (float)delta;

        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        var speed = SPEED;

        if (IsOutOfBreath)
        {
            Stamina += deltaf * StaminaRechargeRateMultiplier;
            if (Stamina >= TotalStaminaInSeconds * OutOfBreathMinimumRefillPercent)
            {
                IsOutOfBreath = false;
            }
        }
        else if (Input.IsActionPressed("run"))
        {
            if (Stamina > 0)
            {
                speed = speed * RunSpeedModifier;
                Stamina -= deltaf;
                if (Stamina <= 0)
                {
                    IsOutOfBreath = true;
                }
            }
        }
        else if (Stamina < TotalStaminaInSeconds)
        {
            Stamina += deltaf * StaminaRechargeRateMultiplier;
        }


        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            IsMoving = true;
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        else
        {
            IsMoving = false;
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
        }
       
        Velocity = velocity;
        MoveAndSlide();

        
    }
}
