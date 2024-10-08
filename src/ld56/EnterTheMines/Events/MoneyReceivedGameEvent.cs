namespace EnterTheMines.EnterTheMines.Events;

public record MoneyReceivedGameEvent(int Amount) : IGameEvent;
