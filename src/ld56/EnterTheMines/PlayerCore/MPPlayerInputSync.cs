using EnterTheMines.EnterTheMines.PlayerCore;
using Godot;

public partial class MPPlayerInputSync : MultiplayerSynchronizer
{
    private const float MOUSE_SENSITIVITY = 0.001f;

    [Export] public Vector2 InputDirection { get; set; }
	[Export] public bool DoJump { get; set; }
    [Export] public bool Running { get; set; }
    [Export] public Vector2 MouseMotion { get; set; }

    private Camera3D camera;
    private MPPlayer mpPlayer;

    /// <summary>
    /// This should only be connected to by code on the server
    /// </summary>
    [Signal]
    public delegate void FlashlightToggledEventHandler();
    public event FlashlightToggledEventHandler OnFlashlightToggled;

    public override void _Ready()
    {
        SetMultiplayerAuthority(GetParent<MPPlayer>().PlayerId);

        if (GetMultiplayerAuthority() == Multiplayer.GetUniqueId())
        {
            GD.Print($"Player Input {Multiplayer.GetUniqueId()} Ready!");
            mpPlayer = GetParent<MPPlayer>();
            camera = mpPlayer.GetNode<Camera3D>("Camera3D");
            Input.MouseMode = Input.MouseModeEnum.Captured;
            camera.Current = true;
        }
        else
        {
            SetProcess(false);
            SetProcessInput(false);
            SetProcessUnhandledInput(false);
        }
    }

    // Called when the node enters the scene tree for the first time.
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        InputDirection = Input.GetVector("left", "right", "forward", "back");
        Running = Input.IsActionPressed("run");
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if(e is InputEventMouseMotion mouseMotion)
        {
            RotateCamera(mouseMotion.Relative);
        }
        else if(e is InputEventKey keyEvent)
        {
            if(keyEvent.IsActionPressed("toggle_flashlight"))
            {
                Rpc(MethodName.ToggleFlashlightRPC);
            }
        }
    }

    public void RotateCamera(Vector2 mouseMotion)
    {
        camera.RotateX(Mathf.Clamp(-mouseMotion.Y * MOUSE_SENSITIVITY, -Mathf.Pi / 2, Mathf.Pi / 2));
        mpPlayer.RotateY(-mouseMotion.X * MOUSE_SENSITIVITY);
    }

    public Basis GetCameraRotationBasis()
    {
        return mpPlayer.GlobalTransform.Basis;
    }

    #region RPCs
    [Rpc(CallLocal = true)]
    public void ToggleFlashlightRPC()
    {
        OnFlashlightToggled?.Invoke();
    }
    #endregion RPCs
}
