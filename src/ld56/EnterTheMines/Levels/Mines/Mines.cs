using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.Levels;
using EnterTheMines.EnterTheMines.PlayerCore;
using Godot;

public partial class Mines : Node3D, ILevel
{
    [Export] private PackedScene playerScene;

    private Node3D playerSpawnLocation;
    private Node3D PlayerContainer;

    public override void _Ready()
	{
        playerSpawnLocation = GetNode<Node3D>("PlayerSpawnLocation");
        PlayerContainer = GetNode<Node3D>("PlayerContainer");

        Input.MouseMode = Input.MouseModeEnum.Captured;
		GameEvents.Raise(new LevelLoadedGameEvent("Mines"));
		GameEvents.Raise(new GameStartGameEvent());
    }

    // Should only be called by the server
    public void SpawnPlayer(int peerId)
    {
        if (!Multiplayer.IsServer()) return;

        var player = playerScene.Instantiate() as MPPlayer;
        player.Name = peerId.ToString();
        player.Position = playerSpawnLocation.Position;
        player.Rotation = playerSpawnLocation.Rotation;
        PlayerContainer.AddChild(player);
    }
}
