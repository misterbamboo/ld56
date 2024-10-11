using Godot;

public partial class MainMenu : Control
{
	private MPClient mpClient;
	private LineEdit ipInput;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        mpClient = GetNode<MPClient>(MPClient.Path);
        ipInput = GetNode<LineEdit>("MarginContainer/VBoxContainer/HBoxContainer/JoinSide/IpInput");
    }


	public void OnHostPressed()
	{
		Hide();
		mpClient.StartHosting();
	}

	public void OnJoinPressed()
	{
		Hide();
		mpClient.JoinGame(ipInput.Text);
	}
}
