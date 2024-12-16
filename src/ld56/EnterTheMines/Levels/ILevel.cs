using EnterTheMines.EnterTheMines.PlayerCore;
using System.Collections.Generic;

namespace EnterTheMines.EnterTheMines.Levels;

public interface ILevel
{
    public void SpawnPlayer(int peerId);

    public List<MPPlayer> GetPlayers();
}
