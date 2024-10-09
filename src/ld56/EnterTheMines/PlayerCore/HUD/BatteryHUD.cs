using Godot;
using Godot.Collections;

namespace EnterTheMines.EnterTheMines.PlayerCore.HUD;

public partial class BatteryHUD : Control
{
    [Export] private Array<Texture2D> textures;
    private Flashlight lights;
    private TextureRect batteryIcon;

    private bool batteryIconFlash = false;

    public override void _Ready()
    {
        lights = FindParent("Player").GetNode<Flashlight>("Neck/Camera3D/Lights");
        batteryIcon = GetNode<TextureRect>("BatteryIcon");
    }

    public void CheckBattery()
    {
        var batteryPercent = lights.BatteryLifeInSeconds / lights.BatteryLifeInSecondsMax;

        if (batteryPercent > 0.8) batteryIcon.Texture = textures[4];
        else if (batteryPercent > 0.6) batteryIcon.Texture = textures[3];
        else if (batteryPercent > 0.4) batteryIcon.Texture = textures[2];
        else if (batteryPercent > 0.1) batteryIcon.Texture = textures[1];
        else if (batteryPercent > 0)
        {
            batteryIconFlash = !batteryIconFlash;
            if (batteryIconFlash)
            {
                batteryIcon.Texture = textures[1];
            }
            else
            {
                batteryIcon.Texture = textures[0];
            }
        }
        else
        {
            batteryIcon.Texture = textures[0];
        }
    }
}
