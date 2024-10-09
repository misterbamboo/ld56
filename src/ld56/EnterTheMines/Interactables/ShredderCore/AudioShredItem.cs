using Godot;

namespace EnterTheMines.EnterTheMines.Interactables.ShredderCore;

public partial class AudioShredItem : AudioStreamPlayer3D
{
    public void GrindPlay()
    {
        Play(0);
    }
}
