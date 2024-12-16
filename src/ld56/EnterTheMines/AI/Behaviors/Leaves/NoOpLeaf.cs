using Godot;

namespace EnterTheMines.EnterTheMines.AI.Behaviors.Leaves;


[Icon("res://EnterTheMines/AI/Behaviors/icons/BTLeaf.svg")]
public partial class NoOpLeaf : BTLeaf
{
    public override BTStatus Tick(double delta, Node actor, Blackboard blackboard)
    {
        return BTStatus.Success;
    }
}
