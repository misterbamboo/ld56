using EnterTheMines.EnterTheMines.PlayerCore;
using Godot;
using System;

namespace EnterTheMines.EnterTheMines.Levels.MPTest;

public partial class MultiplayerTestScene : Node3D
{
	[Export] private PackedScene PlayerScene;
    private PanelContainer MainMenu;
	private LineEdit IpInput;

	private const int PORT = 8055;
	private ENetMultiplayerPeer peer = new ENetMultiplayerPeer();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        MainMenu = GetNode<PanelContainer>("CanvasLayer/MainMenu");
        IpInput = GetNode<LineEdit>("CanvasLayer/MainMenu/MarginContainer/VBoxContainer/HBoxContainer/JoinSide/Adress");
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (e.IsActionPressed("ui_cancel"))
        {
            GetTree().Quit();
        }
    }

    public void OnHostClicked()
	{
		MainMenu.Hide();

		peer.CreateServer(PORT, 4);
		Multiplayer.MultiplayerPeer = peer;
        Multiplayer.PeerConnected += AddPlayer;
		AddPlayer(Multiplayer.GetUniqueId());
    }

	public void OnJoinClicked()
    {
        MainMenu.Hide();

        peer.CreateClient("localhost", PORT);
        Multiplayer.MultiplayerPeer = peer;
    }

	public void AddPlayer(long peerId)
	{
		var player = PlayerScene.Instantiate();
		player.Name = peerId.ToString();
        AddChild(player);
    }
}
