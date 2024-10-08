using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.Player;

public partial class Player : CharacterBody3D
{
    const float SPEED = 4.0f;
    const float JUMP_VELOCITY = 4.5f;
    const double CART_SPEED_MOD = 0.008f;
    const double TIME_TO_SCARE_SECONDS = 0.5f;

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

    private float sensitivity = 0.001f;
    private float runSpeedModifier = 1.5f;
    private GameManager gameManager;

    private Vector2 CameraRotation = new Vector2(90, 0);
    private bool isAttachedToCart = false;

    private bool canMove = false;
    public bool IsInside { get; private set; }

    private bool scared = false;

    private float distanceBetweenFootsteps = 150f;
    private float distanceUntilNextFootsetp = 150f;

    private float totalStaminaInSeconds = 5.0f;
    private float stamina = 5.0f;
    private float staminaRechargeRateMultiplier = 1.2f;
    private float outOfBreathMinimumRefillPercent = 0.75f;
    private bool isOutOfBreath = false;

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
        firstFlashlightLabel = GetNode<Label>("CanvasLayer/Control/FirstFlashlight");
        actionIcon = GetNode<TextureRect>("CanvasLayer/Control/ActionIcon");
        actionLabel = GetNode<Label>("CanvasLayer/Control/ActionLabel");
        hud = GetNode<CanvasLayer>("CanvasLayer");
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
        if (scared)
        {
            if (chaseAudio.VolumeDb < 0)
            {
                chaseAudio.VolumeDb += (float)(80 * (delta / TIME_TO_SCARE_SECONDS));
                if (chaseAudio.VolumeDb > 0) chaseAudio.VolumeDb = 0;
            }


            if (cam.Fov < 110)
            {
                cam.Fov += (float)(20 * (delta / TIME_TO_SCARE_SECONDS));
                if (cam.Fov > 110) cam.Fov = 110;
            }
        }
        else
        {
            if (chaseAudio.VolumeDb > -80)
            {
                chaseAudio.VolumeDb -= (float)(80 * (delta / TIME_TO_SCARE_SECONDS));
                if (chaseAudio.VolumeDb < -80) chaseAudio.VolumeDb = -80;
            }

            if (cam.Fov > 90)
            {
                cam.Fov -= (float)(20 * (delta / TIME_TO_SCARE_SECONDS));
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
                CameraLook(mouseMotion.Relative * sensitivity);
            }
        }
    }

    public void CameraLook(Vector2 movement)
    {
        CameraRotation += movement;
        CameraRotation.Y = (float)Mathf.Clamp(CameraRotation.Y, -1.5, 1.2);

        var transform = Transform;
        transform.Basis = new Basis();
        Transform = transform;

        cam.Basis = new Basis();
        RotateObjectLocal(new Vector3(0, 1, 0), -CameraRotation.X);
        cam.RotateObjectLocal(new Vector3(1, 0, 0), -CameraRotation.Y);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!canMove) return;
        if (isAttachedToCart) return;

        var deltaf = (float)delta;
        float velocityX = 0;
        float velocityY = 0;
        float velocityZ = 0;

        // Add the gravity.
        if (!IsOnFloor())
        {
            Velocity += GetGravity() * deltaf;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocityY = JUMP_VELOCITY;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        var inputDir = Input.GetVector("left", "right", "forward", "back");
        var direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        var speed = SPEED;

        if (isOutOfBreath)
        {
            stamina += deltaf * staminaRechargeRateMultiplier;
            if (stamina >= totalStaminaInSeconds * outOfBreathMinimumRefillPercent)
            {
                isOutOfBreath = false;
            }
        }
        else if (Input.IsActionPressed("run"))
        {
            if (stamina > 0)
            {
                speed = speed * runSpeedModifier;
                stamina -= deltaf;
                if (stamina <= 0)
                {
                    isOutOfBreath = true;
                    PlayOutOfBreathSound();
                }
            }
        }
        else if (stamina < totalStaminaInSeconds)
        {
            stamina += deltaf * staminaRechargeRateMultiplier;
        }

        if(direction != Vector3.Zero)
        {
            velocityX = direction.X * speed;
            velocityZ = direction.Z * speed;
        }
        else
        {
            velocityX = Mathf.MoveToward(Velocity.X, 0, speed);
            velocityZ = Mathf.MoveToward(Velocity.Z, 0, speed);
        }

        Velocity = new Vector3(velocityX, velocityY, velocityZ);

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
