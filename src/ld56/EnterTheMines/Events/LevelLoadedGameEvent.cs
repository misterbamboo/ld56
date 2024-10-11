namespace EnterTheMines.EnterTheMines.Events;

public record LevelLoadedGameEvent(string Name) : IGameEvent;
