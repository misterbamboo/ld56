using System;
using Godot;
using System.Collections.Generic;

namespace EnterTheMines.EnterTheMines.Events
{
    public interface IGameEvent;

    public class EndDayGameEvent : IGameEvent;
    public class StartDayGameEvent : IGameEvent;
    public class GameStartGameEvent : IGameEvent;
    public class GameOverGameEvent : IGameEvent;
    public class  TitleScreenGameEvent : IGameEvent;
    public class StartChaseGameEvent : IGameEvent;
    public class StopChaseGameEvent : IGameEvent;
    public class GameTimeoutGameEvent : IGameEvent;
    public class HitPlayerGameEvent : IGameEvent;

    /// <summary>
    /// http://msdn.microsoft.com/en-gb/magazine/ee236415.aspx#id0400046
    /// </summary>
    public static class GameEvents
    {
        private static List<Delegate> actions = new List<Delegate>();

        [Obsolete("Not obsolete but don't forget to unregister!")]
        public static void Register<T>(Action<T> callback) where T : IGameEvent
        {
            actions.Add(callback);
        }

        public static void UnRegister<T>(Action<T> callback) where T : IGameEvent
        {
            actions.Remove(callback);
        }

        public static void ClearCallbacks()
        {
            actions.Clear();
        }

        public static void Raise<T>(T args) where T : IGameEvent
        {
            GD.PrintRich($"[color=cyan][b]{typeof(T).Name} event raised[/b][/color]");
            /*
             *  Uncomment this to enable registering full classes as handlers
                foreach (var handler in Container.GetAllInstances<IHandle<T>>())
                {
                    handler.Handle(args);
                }
            */

            if (actions != null)
            {
                foreach (var action in actions)
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
        }
    }
}
