using Godot;
using System.Collections.Generic;

namespace EnterTheMines.EnterTheMines.AI.Behaviors
{
    [Tool]
    [Icon("res://EnterTheMines/AI/Behaviors/icons/BTLeaf.svg")]
    public abstract partial class BTLeaf : BTNode
    {
        public override string[] _GetConfigurationWarnings()
        {
            List<string> warnings = [];

            var parent = GetParent();
            var children = GetChildren();

            if (parent is not BTNode && parent is not BehaviorTree)
            {
                warnings.Add("BTLeaf node must be a child of BTBehaviour or BehaviorTree node.");
            }

            if (children.Count > 0)
            {
                warnings.Add("BTLeaf node must not have any children.");
            }

            return warnings.ToArray();
        }
    }
}
