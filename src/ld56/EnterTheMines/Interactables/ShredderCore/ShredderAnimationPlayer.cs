using Godot;

namespace EnterTheMines.EnterTheMines.Interactables.ShredderCore;

public partial class ShredderAnimationPlayer : AnimationPlayer
{
    public override void _Ready()
    {
        Play("grind");
    }

    public void OnAnimationFinished(StringName animName)
    {
        Play("grind");
    }
}
