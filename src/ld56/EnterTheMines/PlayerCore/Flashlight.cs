using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.Services;
using Godot;

namespace EnterTheMines.EnterTheMines.PlayerCore;

public partial class Flashlight : Node3D
{
    private const float AMBIENT_STRENGTH_ON = 0.15f;
    private const float AMBIENT_STRENGTH_OFF = 0.05f;

    private GameManager gameManager;
    private SpotLight3D flashlight;
    private OmniLight3D ambientLight;
    private AudioStreamPlayer3D audioOn;
    private AudioStreamPlayer3D audioOff;

    private bool flashlightOn = false;
    public float BatteryLifeInSecondsMax { get; private set; } = 120.0f;
    public float BatteryLifeInSeconds { get; private set; } = 120.0f;

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>(GameManager.Path);
        flashlight = GetNode<SpotLight3D>("Flashlight");
        ambientLight = GetNode<OmniLight3D>("AmbientLight");
        audioOn = GetNode<AudioStreamPlayer3D>("audio_flash_on");
        audioOff = GetNode<AudioStreamPlayer3D>("audio_flash_off");

        GameEvents.Register<GameStartGameEvent>(OnGameStart);
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("toggle_flashlight"))
        {
            ToggleFlashlight();
        }
    }

    public void OnGameStart(GameStartGameEvent e)
    {
        if(!gameManager.FirstFlashlight)
        {
            RemoveFlashlightPrompt();
        }
    }

    public override void _Process(double delta)
    {
        if (flashlightOn && BatteryLifeInSeconds > 0)
        {
            BatteryLifeInSeconds -= (float)delta;
            if (BatteryLifeInSeconds <= 0)
            {
                BatteryLifeInSeconds = 0;
                TurnOff();
            }
        }
    }

    public void ToggleFlashlight()
    {
        if(BatteryLifeInSeconds <= 0) return;
        
        if (!flashlightOn)
        {
            TurnOn();
            audioOn.Play(0);
        }
        else
        {
            TurnOff();
            audioOff.Play(0);
        }
    }

    private void TurnOn()
    {
        if(gameManager.FirstFlashlight)
        {
            gameManager.TurnFlashlightOnForTheFirstTime();
            RemoveFlashlightPrompt();
        }

        flashlightOn = true;
        flashlight.Visible = true;
        ambientLight.LightEnergy = AMBIENT_STRENGTH_ON;
    }

    private void TurnOff()
    {
        flashlightOn = false;
        flashlight.Visible = false;
        ambientLight.LightEnergy = AMBIENT_STRENGTH_OFF;
    }

    private void RemoveFlashlightPrompt()
    {
        FindParent("Player").GetNode<Label>("PlayerHUD/Control/FirstFlashlight").Visible = false;
    }
}
