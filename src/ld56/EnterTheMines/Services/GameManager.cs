using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.Player;
using Godot;
using System;
using System.Collections.Generic;

namespace EnterTheMines.Services;

public partial class GameManager : Node
{
	public const string Path = "/root/GameManager";

	private Dictionary<string, Action> registeredCallbacks = [];

	private bool playerAlive = true;

	public Player player { get; private set; }

	private bool firstFlashlight = true;

	private int weekDuration = 3;
	private int currentDay = 1;

	private float firstQuota = 120;
	private float quota = 120;

	public int uncashedInMoney { get; private set; }

	public int money { get; private set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void GiveMoney(int amount)
	{
        uncashedInMoney += amount;
        GameEvents.Raise(new MoneyReceivedGameEvent(amount));
    }

	public void RegisterPlayer(Player player)
	{
		this.player = player;
    }

	public void ResetGame(bool newGame)
	{
		GetTree().ReloadCurrentScene();
		GameEvents.ClearCallbacks();
        playerAlive = true;

		if (newGame)
		{
			quota = firstQuota;
			money = 0;
            uncashedInMoney = 0;
        }
		else
		{
			GameEvents.Raise(new GameStartGameEvent());
        }
    }

	public void NextDay()
	{
		currentDay++;
		if (currentDay > weekDuration+1)
        {
			currentDay = 1;
			if (QuotaReached)
			{
				quota *= 1.7f;
				CashInMoney();
				ProceedToNextDay();
			}
			else
			{
				GameEvents.Raise(new GameOverGameEvent());
            }
        }
        else
        {
			ProceedToNextDay();
        }
    }

	public bool QuotaReached => uncashedInMoney >= quota;

	public void ProceedToNextDay()
	{
		if (player.IsInside || !playerAlive)
		{
			uncashedInMoney = 0;
		}

		ResetGame(false);
    }

    public void CashInMoney()
    {
        money += uncashedInMoney;
        uncashedInMoney = 0;
    }

	public bool IsPlayerHittable => playerAlive;

	public void PlayerDie()
	{
		if (!playerAlive) return;
        playerAlive = false;
		GameEvents.Raise(new EndDayGameEvent());
    }

	// quick method pour set le action a display dans le UI du player
	public void SetAction(string label)
	{
		player.SetAction(label);
    }
}
