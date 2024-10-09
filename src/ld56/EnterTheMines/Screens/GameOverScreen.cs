using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.Services;
using Godot;
using System;

namespace EnterTheMines.EnterTheMines.Screens;

public partial class GameOverScreen : Control
{
	private GameManager gameManager;
	private AnimationPlayer animationPlayer;

    public override void _Ready()
	{
        gameManager = GetNode<GameManager>(GameManager.Path);
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        GameEvents.Register<GameOverGameEvent>(StartAnimation);
		HideGameoverScreen();
	}

	public void StartAnimation(GameOverGameEvent _)
	{
		UnhideGameoverScreen();
		animationPlayer.Seek(0, true);
		animationPlayer.Play("gameover_move_up");
	}

	public void OnButtonPressed()
	{
		gameManager.ResetGame(true);
	}

	public void HideGameoverScreen()
	{
		Hide();
	}

	public void UnhideGameoverScreen()
	{
		Show();
	}
}
