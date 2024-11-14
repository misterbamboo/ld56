using Godot;
using System.Collections.Generic;
using System.Linq;

namespace EnterTheMines.EnterTheMines.AI.Behaviors;

[Tool]
[Icon("res://EnterTheMines/AI/Behaviors/icons/BTComposite.svg")]
public abstract partial class BTComposite : BTNode
{
    public List<BTNode> Leaves { get; set; } = new List<BTNode>();

    public override void _Ready()
    {
        Leaves = GetChildren().Cast<BTNode>().ToList();    
    }

    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = [];


        var parent = GetParent();
        var children = GetChildren();


        if (parent is not BTComposite && parent is not BehaviorTree && parent is not BTDecorator)
        {
            warnings.Add("BTComposite node must be a child of BTComposite, BTDecorator or BTRoot node.");
        }

        if (children.Count == 0)
        {
            warnings.Add("BTComposite node must have at least one child.");
        }


        if (children.Count == 1)
        {
            warnings.Add("BTComposite node should have more than one child.");
        }

        return warnings.ToArray();
    }
}
