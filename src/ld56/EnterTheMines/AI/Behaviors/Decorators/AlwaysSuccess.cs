using Godot;

namespace EnterTheMines.EnterTheMines.AI.Behaviors.Decorators;

[Icon("res://EnterTheMines/AI/Behaviors/icons/BTDecoratorSucceed.svg")]
public partial class AlwaysSuccess : BTDecorator
{
    public override BTStatus Tick(double delta, Node actor, Blackboard blackboard)
    {
        return BTStatus.Success;
    }
}
