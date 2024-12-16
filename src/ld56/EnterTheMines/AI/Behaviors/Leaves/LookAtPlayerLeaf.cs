using EnterTheMines.EnterTheMines.AI.Behaviors;
using EnterTheMines.EnterTheMines.Levels;
using Godot;

[Icon("res://EnterTheMines/AI/Behaviors/icons/BTLeaf.svg")]
public partial class LookAtPlayerLeaf : BTLeaf
{
    public override BTStatus Tick(double delta, Node actor, Blackboard blackboard)
    {
        var actor3D = actor as Node3D;
        var players = (FindParent("Level") as ILevel).GetPlayers();

        if (actor3D == null || players == null || players.Count == 0)
        {
            return BTStatus.Failure;
        }

        Node3D closestPlayer = null;
        float closestDistanceSquared = float.MaxValue;

        foreach (Node3D player in players)
        {
            float distanceSquared = actor3D.GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
            if (distanceSquared < closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestPlayer = player;
            }
        }

        if (closestPlayer != null)
        {
            Vector3 direction = (closestPlayer.GlobalPosition - actor3D.GlobalPosition).Normalized();
            actor3D.LookAt(closestPlayer.GlobalPosition, Vector3.Up);
            return BTStatus.Success;
        }

        return BTStatus.Failure;
    }
}
