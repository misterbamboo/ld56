using EnterTheMines.EnterTheMines.Events;
using Godot;

public partial class MPClient : Node
{
    public const string Path = "/root/MPClient";
    private const int PORT = 8055;
    private ENetMultiplayerPeer peer = new ENetMultiplayerPeer();

    [Signal]
    public delegate void PeerConnectedEventHandler(int peerId);
    public event PeerConnectedEventHandler OnPeerConnected;

    public void StartHosting()
    {
        peer.CreateServer(PORT, 4);
        Multiplayer.MultiplayerPeer = peer;
        Multiplayer.PeerConnected += (id) => OnPeerConnected?.Invoke((int)id);
        GameEvents.Raise(new StartedHostingSessionGameEvent());
    }
  

    public void JoinGame(string ipAddress)
    {
        peer.CreateClient("localhost", PORT);
        Multiplayer.MultiplayerPeer = peer;
    }

    //public void AddPlayer(long peerId)
    //{
    //    var player = PlayerScene.Instantiate();
    //    player.Name = peerId.ToString();
    //    AddChild(player);
    //}
}
