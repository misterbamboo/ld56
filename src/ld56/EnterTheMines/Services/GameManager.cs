using EnterTheMines.EnterTheMines.Events;
using EnterTheMines.EnterTheMines.PlayerCore;
using Godot;
using System;
using System.Collections.Generic;

namespace EnterTheMines.EnterTheMines.Services;

public partial class GameManager : Node
{
	public const string Path = "/root/GameManager";

	private Dictionary<string, Action> registeredCallbacks = [];

	public bool PlayerAlive { get; private set; } = true;

	public Player Player { get; private set; }

	public bool FirstFlashlight { get; private set; } = true;

	public int WeekDuration { get; private set; } = 3;
	public int CurrentDay { get; private set; } = 1;

	private float firstQuota = 120;

	public float Quota { get; private set; } = 120;

	public int UncashedInMoney { get; private set; }

	public int Money { get; private set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void GiveMoney(int amount)
	{
        UncashedInMoney += amount;
        GameEvents.Raise(new MoneyReceivedGameEvent(amount));
    }

	public void RegisterPlayer(Player player)
	{
		this.Player = player;
    }

	public void ResetGame(bool newGame)
	{
		GetTree().ReloadCurrentScene();
		GameEvents.ClearCallbacks();
        PlayerAlive = true;

		if (newGame)
		{
			Quota = firstQuota;
			Money = 0;
            UncashedInMoney = 0;
        }
		else
		{
			GameEvents.Raise(new GameStartGameEvent());
        }
    }

	public void NextDay()
	{
		CurrentDay++;
		if (CurrentDay > WeekDuration+1)
        {
			CurrentDay = 1;
			if (QuotaReached)
			{
				Quota *= 1.7f;
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

	public bool QuotaReached => UncashedInMoney >= Quota;

	public void ProceedToNextDay()
	{
		if (Player.IsInside || !PlayerAlive)
		{
			UncashedInMoney = 0;
		}

		ResetGame(false);
    }

    public void CashInMoney()
    {
        Money += UncashedInMoney;
        UncashedInMoney = 0;
    }

	public bool IsPlayerHittable => PlayerAlive;

	public void PlayerDie()
	{
		if (!PlayerAlive) return;
        PlayerAlive = false;
		GameEvents.Raise(new EndDayGameEvent());
    }

	// quick method pour set le action a display dans le UI du player
	public void SetAction(string label)
	{
		Player.SetAction(label);
    }

	public void TurnFlashlightOnForTheFirstTime()
    {
        FirstFlashlight = false;
    }
}
