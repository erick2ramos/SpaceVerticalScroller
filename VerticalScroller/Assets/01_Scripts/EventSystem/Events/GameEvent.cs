using UnityEngine;
using System.Collections;

namespace BaseSystems.EventSystem
{
    /// <summary>
    /// Used to broadcast a trigger event, only uses a string so it can be customized at will
    /// </summary>
    public struct GameEvent
    {
        public string EventName;

        public GameEvent(string eventName)
        {
            EventName = eventName;
        }

        static GameEvent e;

        public static void Trigger(string eventName)
        {
            e.EventName = eventName;
            EventManager.Trigger(e);
        }
    }
}