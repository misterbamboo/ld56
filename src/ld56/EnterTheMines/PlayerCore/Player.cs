using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.PlayerCore;

public partial class Player : CharacterBody3D
{
    const float SPEED = 4.0f;
    const float JUMP_VELOCITY = 4.5f;
    const float CART_SPEED_MOD = 0.008f;
    const float TIME_TO_SCARE_SECONDS = 0.5f;

    #region references
    private Node3D neck;
    private Camera3D cam;
    private AudioStreamPlayer3D footstep;
    private Label firstFlashlightLabel;
    private TextureRect actionIcon;
    private Label actionLabel;
    private CanvasLayer hud;
    private AudioStreamPlayer3D outsideAudio;
    private AudioStreamPlayer3D heavyAudio;
    private AudioStreamPlayer3D chaseAudio;
    private AudioStreamPlayer3D outOfBreathAudio;
    #endregion

    private float sensitivity = 1f;
    private float runSpeedModifier = 1.5f;
    private GameManager gameManager;

    private Vector2 CameraRotation = new Vector2(90, 0);
    private bool isAttachedToCart = false;

    private bool canMove = false;
    public bool IsInside { get; private set; }

    private bool scared = false;

    private float distanceBetweenFootsteps = 150f;
    private float distanceUntilNextFootsetp = 150f;

    public float TotalStaminaInSeconds { get; private set; } = 5.0f;
    public float Stamina { get; private set; } = 5.0f;
    public float StaminaRechargeRateMultiplier { get; private set; } = 1.2f;
    public float OutOfBreathMinimumRefillPercent { get; private set; } = 0.75f;
    public bool IsOutOfBreath { get; private set; } = false;

    public Vector2 MouseDelta { get; private set; } = Vector2.Zero;

    private string action = "";

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>(GameManager.Path);

        GD.PrintRich("[color=green] playerReady![/color]");
        GameEvents.Register<GameStartGameEvent>(OnGameStart);
        GameEvents.Register<GameOverGameEvent>(OnGameOver);
        GameEvents.Register<EndDayGameEvent>(OnEndDay);
        GameEvents.Register<HitPlayerGameEvent>(OnReceiveHit);
        GameEvents.Register<StartChaseGameEvent>(OnStartChase);
        GameEvents.Register<StopChaseGameEvent>(OnStopChase);


        neck = GetNode<Node3D>("Neck");
        cam = GetNode<Camera3D>("Neck/Camera3D");
        footstep = GetNode<AudioStreamPlayer3D>("Footsteps");
        firstFlashlightLabel = GetNode<Label>("PlayerHUD/Control/FirstFlashlight");
        actionIcon = GetNode<TextureRect>("PlayerHUD/Control/ActionIcon");
        actionLabel = GetNode<Label>("PlayerHUD/Control/ActionLabel");
        hud = GetNode<CanvasLayer>("PlayerHUD");
        outsideAudio = GetNode<AudioStreamPlayer3D>("AudioOutsideAmbiance");
        heavyAudio = GetNode<AudioStreamPlayer3D>("AudioHeavyAmbiance");
        chaseAudio = GetNode<AudioStreamPlayer3D>("AudioChase");
        outOfBreathAudio = GetNode<AudioStreamPlayer3D>("AudioOutOfBreath");

        gameManager.RegisterPlayer(this);
    }

    public void OnGameStart(GameStartGameEvent e)
    {
        canMove = true;
        DisplayHud();
    }

    public void OnStartChase(StartChaseGameEvent e)
    {
        scared = true;
    }

    public void OnStopChase(StopChaseGameEvent e)
    {
        scared = false;
    }

    public void OnGameOver(GameOverGameEvent e)
    {
        UncaptureMouse();
    }

    public void OnEndDay(EndDayGameEvent e)
    {
        UncaptureMouse();
    }

    public void SetAction(string label)
    {
        action = label;
    }

    public override void _Process(double delta)
    {
        var deltaf = (float)delta;
        CameraLook(deltaf);
        if (scared)
        {
            if (chaseAudio.VolumeDb < 0)
            {
                chaseAudio.VolumeDb += 80 * (deltaf / TIME_TO_SCARE_SECONDS);
                if (chaseAudio.VolumeDb > 0) chaseAudio.VolumeDb = 0;
            }


            if (cam.Fov < 110)
            {
                cam.Fov += 20 * (deltaf / TIME_TO_SCARE_SECONDS);
                if (cam.Fov > 110) cam.Fov = 110;
            }
        }
        else
        {
            if (chaseAudio.VolumeDb > -80)
            {
                chaseAudio.VolumeDb -= 80 * (deltaf / TIME_TO_SCARE_SECONDS);
                if (chaseAudio.VolumeDb < -80) chaseAudio.VolumeDb = -80;
            }

            if (cam.Fov > 90)
            {
                cam.Fov -= 20 * (deltaf / TIME_TO_SCARE_SECONDS);
                if (cam.Fov < 90) cam.Fov = 90;
            }
        }

        if (Input.IsActionPressed("action") || action == "")
        {
            actionLabel.Visible = false;
            actionIcon.Visible = false;
        }
        else
        {
            actionIcon.Visible = true;
            actionLabel.Text = action;
            actionLabel.Visible = true;
        }
    }

    public void UncaptureMouse()
    {
        if (chaseAudio.Playing) chaseAudio.Stop();
        if (heavyAudio.Playing) heavyAudio.Stop();
        if (outsideAudio.Playing) outsideAudio.Stop();

        HideHud();
        action = ""; // stop displaying action by setting it to empty string
        canMove = false;
        Input.SetMouseMode(Input.MouseModeEnum.Visible);
    }

    public void DisplayHud()
    {
        hud.Visible = true;
    }

    public void HideHud()
    {
        hud.Visible = false;
    }

    public void OnReceiveHit(HitPlayerGameEvent e)
    {
        gameManager.PlayerDie();
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (e.IsActionPressed("ui_cancel"))
        {
            if (Input.GetMouseMode() == Input.MouseModeEnum.Visible)
            {
                GetTree().Quit();
            }
            Input.SetMouseMode(Input.MouseModeEnum.Visible);
        }
        else if (e.IsActionPressed("action"))
        {
            Input.SetMouseMode(Input.MouseModeEnum.Captured);
        }

        if (Input.GetMouseMode() == Input.MouseModeEnum.Captured)
        {
            if (e is InputEventMouseMotion mouseMotion)
            {
                MouseDelta = mouseMotion.Relative;
                //CameraLook(mouseMotion.Relative * sensitivity);
            }
        }
    }

    public void CameraLook(float deltaf)
    {
        var upDown = Mathf.Clamp(-MouseDelta.Y * sensitivity * deltaf, -1.5f, 1.2f);
        var leftRight = -MouseDelta.X * sensitivity * deltaf;

        cam.RotateObjectLocal(new Vector3(1, 0, 0), upDown);
        RotateObjectLocal(new Vector3(0, 1, 0), leftRight);

        // reset mouse delta so view doesn't keep spinning when mouse has stopped moving
        MouseDelta = new Vector2();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!canMove) return;
        if (isAttachedToCart) return;

        var deltaf = (float)delta;
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            Velocity += GetGravity() * deltaf;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = JUMP_VELOCITY;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        var inputDir = Input.GetVector("left", "right", "forward", "back");
        var direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

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
                speed = speed * runSpeedModifier;
                Stamina -= deltaf;
                if (Stamina <= 0)
                {
                    IsOutOfBreath = true;
                    PlayOutOfBreathSound();
                }
            }
        }
        else if (Stamina < TotalStaminaInSeconds)
        {
            Stamina += deltaf * StaminaRechargeRateMultiplier;
        }

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
        }

        Velocity = velocity;

        distanceBetweenFootsteps -= Velocity.Length();
        if (distanceUntilNextFootsetp < 0)
        {
            distanceUntilNextFootsetp = distanceBetweenFootsteps;
            footstep.Play();
        }

        MoveAndSlide();
    }

    public void AttachToCart()
    {
        isAttachedToCart = true;
    }

    public void DetachFromCart()
    {
        isAttachedToCart = false;
    }

    public void PlayInsideSound(Node3D body)
    {
        GD.Print("inside!");
        IsInside = true;
        if (heavyAudio.Playing) return;
        heavyAudio.Play(0);
        outsideAudio.Stop();
    }

    public void PlayOutsideSound(Node3D body)
    {
        GD.Print("outside!");
        IsInside = false;
        if (outsideAudio.Playing) return;
        outsideAudio.Play(0);
        heavyAudio.Stop();
    }

    public void PlayOutOfBreathSound()
    {
        outOfBreathAudio.Play(0);
    }
}
