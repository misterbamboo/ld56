using Godot;

namespace EnterTheMines.EnterTheMines.AI.Behaviors.Composites
{
    /// <summary>
    /// Returns at first successful child. continues to next child otherwise.
    /// Fails if all children fail.
    /// </summary>
    [Tool]
    [Icon("res://EnterTheMines/AI/Behaviors/icons/BTCompositeSelector.svg")]
    public partial class BTSelector : BTComposite
    {
        private int currentLeaf = 0;

        public override BTStatus Tick(double delta, Node actor, Blackboard blackboard)
        {
            if(currentLeaf > Leaves.Count - 1)
            {
                currentLeaf = 0;
                return BTStatus.Failure;
            }

            var response = Leaves[currentLeaf].Tick(delta, actor, blackboard);

            if (response == BTStatus.Running) return response;

            if (response == BTStatus.Success)
            {
                currentLeaf = 0;
                return response;
            }

            currentLeaf++;

            return BTStatus.Running;
        }
    }
}
