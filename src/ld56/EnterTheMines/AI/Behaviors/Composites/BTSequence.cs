using Godot;

namespace EnterTheMines.EnterTheMines.AI.Behaviors.Composites;

[Tool]
[Icon("res://EnterTheMines/AI/Behaviors/icons/BTCompositeSequence.svg")]
public partial class BTSequence : BTComposite
{
    private int currentLeaf = 0;

    public override BTStatus Tick(double delta, Node Actor, Blackboard blackboard)
    {
        if(currentLeaf >= Leaves.Count -1)
        {
            currentLeaf = 0;
            return BTStatus.Success;
        }

        var status = Leaves[currentLeaf].Tick(delta, Actor, blackboard);

        if (status == BTStatus.Running) return BTStatus.Running; 
      
        if (status == BTStatus.Failure)
        {
            currentLeaf = 0;
            return BTStatus.Failure;
        }

        currentLeaf++;
        return BTStatus.Running;
    }
}
