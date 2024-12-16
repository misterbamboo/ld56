using Godot;
using System.Collections.Generic;

namespace EnterTheMines.EnterTheMines.AI.Behaviors;

[Icon("res://EnterTheMines/AI/Behaviors/icons/BTRoot.svg")]
public partial class BehaviorTree : Node
{
	public enum ProcessTypes
	{
		Physics,
		Idle
	}

	[Export] public bool AutoStart { get; set; } = false;

	[Export] public ProcessTypes ProcessType { get; set; } = ProcessTypes.Physics;

	[Export] public Node Actor { get; set; }

	[Export] public Blackboard Blackboard { get; set; }

	private bool active = false;
	private BTStatus currentStatus = BTStatus.Success; 
	private BTNode entryPoint = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		GD.Print("BEHAVIOR TREE READY BOIS!!!");
        // Do not run in editor
        if (Engine.IsEditorHint() || !Multiplayer.IsServer())
		{
			GD.Print("Behavior tree Canceled because not on server");
			SetPhysicsProcess(false);
			SetProcess(false);
			return;
		}

        entryPoint = GetChild(0) as BTNode;
		if (entryPoint == null)
		{
			throw new System.Exception("Behavior Tree must have a root node");
        }

		Blackboard ??= new Blackboard();
        if (AutoStart) active = true;
		SetupProcessing();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Tick(delta);
	}

    public override void _PhysicsProcess(double delta)
    {
        Tick(delta);
    }

	public void Tick(double delta)
	{
		if (!active) return;

        currentStatus = entryPoint.Tick(delta, Actor, Blackboard);
    }

    public void SetupProcessing()
	{
		SetPhysicsProcess(ProcessType == ProcessTypes.Physics);
        SetProcess(ProcessType == ProcessTypes.Idle);
    }

	public override string[] _GetConfigurationWarnings()
	{
        List<string> warnings = [];


		var children = GetChildren();


		if (children.Count == 0)
		{
			warnings.Add("Behaviour Tree needs to have one Behaviour child.");
		}
		else if (children.Count == 1)
		{
			if (children[0] is not BTNode)
			{
				warnings.Add("The child of Behaviour Tree needs to be a Behaviour.");
			}
		}
		else if(children.Count > 1)
		{
			warnings.Add("Behaviour Tree can have only one Behaviour child.");
		}

		return warnings.ToArray();
    }
}
