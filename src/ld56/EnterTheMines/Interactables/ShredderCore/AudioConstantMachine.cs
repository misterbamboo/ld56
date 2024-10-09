using EnterTheMines.EnterTheMines.Events;
using Godot;

namespace EnterTheMines.EnterTheMines.Interactables.ShredderCore;

public partial class AudioConstantMachine : AudioStreamPlayer3D
{

    public override void _Ready()
    {
        GameEvents.Register<GameStartGameEvent>((_)=> { Play(0); });
        GameEvents.Register<EndDayGameEvent>((_) => { Stop(); });
        GameEvents.Register<TitleScreenGameEvent>((_) => { Stop(); });
    }
}
