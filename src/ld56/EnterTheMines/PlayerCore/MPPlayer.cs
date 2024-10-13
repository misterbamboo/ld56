using EnterTheMines.EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.PlayerCore;

public partial class MPPlayer : CharacterBody3D
{
    public const float SPEED = 4.0f;
    public const float JUMP_VELOCITY = 4.5f;
    public const float CART_SPEED_MOD = 0.008f;
    public const float TIME_TO_SCARE_SECONDS = 0.5f;

    private GameManager gameManager;
    private Camera3D camera;
    private SpotLight3D flashlight;
    private AudioStreamPlayer3D audioOn;
    private AudioStreamPlayer3D audioOff;
    private AnimationNodeStateMachinePlayback animationStateMachine;
    private MPPlayerInputSync mpPlayerInputSync;

    private float sensitivity = 0.001f;
    public float RunSpeedModifier { get; private set; } = 1.5f;

    private Vector2 CameraRotation = new Vector2(90, 0);
    public float TotalStaminaInSeconds { get; private set; } = 5.0f;
    public float Stamina { get; set; } = 5.0f;
    public float StaminaRechargeRateMultiplier { get; private set; } = 1.2f;
    public float OutOfBreathMinimumRefillPercent { get; private set; } = 0.75f;
    public bool IsOutOfBreath { get; set; } = false;
    public bool IsMoving { get; private set; } = false;

    public const float JumpVelocity = 4.5f;

    public int wakeupFrames = 1;

    private int _playerId = 1;

    [Export] public string currentAnimation = "idle";

    [Export] public int PlayerId = 1;

    public void InitMP(int playerId)
    {
        Name = playerId.ToString();
        PlayerId = playerId;
    }

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>(GameManager.Path);
        camera = GetNode<Camera3D>("Camera3D");
        animationStateMachine = GetNode<AnimationTree>("AnimationTree").Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
        mpPlayerInputSync = GetNode<MPPlayerInputSync>("MPPlayerInputSync");
        flashlight = GetNode<SpotLight3D>("Camera3D/Lights/Flashlight");
        audioOn = GetNode<AudioStreamPlayer3D>("Camera3D/Lights/AudioFlashlightOn");
        audioOff = GetNode<AudioStreamPlayer3D>("Camera3D/Lights/AudioFlashlightOff");

        GD.Print($"{Multiplayer.GetUniqueId()} ready for {PlayerId}");

        if(!Multiplayer.IsServer())
        {
            SetProcess(false);
        }
        else
        {
            mpPlayerInputSync.OnFlashlightToggled += OnToggleFlashlight;
        }
        GD.PrintRich($"[color=green] MPPlayer {Name} Ready![/color]");
    }

   

    public override void _UnhandledInput(InputEvent e)
    {
        if (!IsMultiplayerAuthority()) return;

        //if (e is InputEventMouseMotion mouseMotion)
        //{
        //    RotateY(-mouseMotion.Relative.X * sensitivity);
        //    camera.RotateX(Mathf.Clamp(-mouseMotion.Relative.Y * sensitivity, -Mathf.Pi/2, Mathf.Pi/2));
        //}
    }

    public void OnToggleFlashlight()
    {
        if (!Multiplayer.IsServer()) return;

        Rpc(MethodName.ToggleFlashlightRPC);
    }


    public override void _PhysicsProcess(double delta)
    {
        float deltaf = (float)delta;

        if (Multiplayer.IsServer())
        {
            ApplyInput(deltaf);
        }
        else 
        {
            Animate(currentAnimation, deltaf);
        }
    }

    public void ApplyInput(float deltaf)
    {
        // for some reason le jeu brise si on attend pas un frame avant de bouger
        // le joueur se fait fling a l'autre bout de la map
        if (wakeupFrames > 0)
        {
            wakeupFrames--;
            return;
        }

        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * deltaf;
        }

        // Handle Jump.
        if (mpPlayerInputSync.DoJump && IsOnFloor())
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
        else if (mpPlayerInputSync.Running)
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

        Vector3 direction = (Transform.Basis * new Vector3(mpPlayerInputSync.InputDirection.X, 0, mpPlayerInputSync.InputDirection.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            Animate("run", deltaf);
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        else
        {
            Animate("idle", deltaf);
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public void Animate(string animation, float delta)
    {
        currentAnimation = animation;
        animationStateMachine.Travel(animation);
    }

    #region RPCs
    /// <summary>
    /// Only call this from the server
    /// </summary>
    public void SpawnAtPositionServer(Node3D spawnLocation)
    {
        if (!Multiplayer.IsServer()) return;
        Rpc(MethodName.SpawnAtPosition, spawnLocation.Position, spawnLocation.Rotation);
    }

    [Rpc(CallLocal = true)]
    private void SpawnAtPosition(Vector3 spawnLocation, Vector3 spawnRotation)
    {
        Position = spawnLocation;
        Rotation = spawnRotation;
    }

    [Rpc(CallLocal = true)]
    private void ToggleFlashlightRPC()
    {
        flashlight.Visible = !flashlight.Visible;
        if(flashlight.Visible)
        {
            audioOn.Play(0);
        }
        else
        {
            audioOff.Play(0);
        }
    }

    #endregion RPCs
}
