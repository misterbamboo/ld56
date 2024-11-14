using Godot;

namespace EnterTheMines.EnterTheMines.AI.Behaviors;

public abstract partial class BTNode : Node
{
    public abstract BTStatus Tick(double delta, Node actor, Blackboard blackboard);
}
