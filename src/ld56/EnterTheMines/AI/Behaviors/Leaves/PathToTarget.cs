using EnterTheMines.EnterTheMines.AI.Behaviors;
using Godot;
using System;

[Icon("res://EnterTheMines/AI/Behaviors/icons/BTLeaf.svg")]
public partial class FindDirectionToTarget : BTLeaf
{
    [Export] public NavigationAgent3D NavigationAgent3D { get; set; }

    public override BTStatus Tick(double delta, Node actor, Blackboard blackboard)
    {
        var target = blackboard.GetValue("Target").AsVector3();
        if (target == null)
        {
            return BTStatus.Failure;
        }

        if(!NavigationAgent3D.GetNavigationMap().IsValid)
        {
            return BTStatus.Failure;
        }

        NavigationAgent3D.TargetPosition = target;
        var direction = NavigationAgent3D.GetNextPathPosition().Normalized();

        blackboard.SetValue("Direction", direction);

        return BTStatus.Success;
    }
}
