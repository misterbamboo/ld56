using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.Levels;
using EnterTheMines.EnterTheMines.PlayerCore;
using Godot;

public partial class Mines : Node3D, ILevel
{
	[Export] private PackedScene playerScene;

	private Node3D PlayerContainer;
	private Node3D PlayerSpawnLocation;

	public override void _Ready()
	{
		PlayerContainer = GetNode<Node3D>("PlayerContainer");
		PlayerSpawnLocation = GetNode<Node3D>("PlayerContainer/PlayerSpawnLocation");
	}

	// Should only be called by the server
	public void SpawnPlayer(int peerId)
	{
		if (!Multiplayer.IsServer()) return;

		var player = playerScene.Instantiate() as MPPlayer;
		player.InitMP(peerId);

		PlayerContainer.AddChild(player, true);
		player.SpawnAtPositionServer(PlayerSpawnLocation);
	}
}
