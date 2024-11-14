using Godot;
using System;
using System.Collections.Generic;

namespace EnterTheMines.EnterTheMines.AI.Behaviors;

[Tool]
[Icon("res://EnterTheMines/AI/Behaviors/icons/BTDecorator.svg")]
public abstract partial class BTDecorator : BTNode
{
    public BTNode Leaf { get; set; }

    public override void _Ready()
    {
        Leaf = GetLeaf();
    }

    public BTNode GetLeaf()
    {
        if (GetChildCount() == 0) return null;

        return GetChild(0) as BTNode;
    }

    public override string[] _GetConfigurationWarnings()
    {

        List<string> warnings = [];
        var parent = GetParent();
        var children = GetChildren();

        if (parent is not BTComposite && parent is not BehaviorTree)
        {
            warnings.Add("Decorator node should be a child of a composite node or the root node.");
        }

        if (children.Count == 0)
        {
            warnings.Add("Decorator node should have a child.");
        }
        else if(children.Count > 1)
        {
            warnings.Add("Decorator node should have only one child.");
        }
        else if(children[0] is not BTNode)
        {
            warnings.Add("Decorator node should have a BTBehaviour node as a child.");
        }

        return warnings.ToArray();
    }
}
