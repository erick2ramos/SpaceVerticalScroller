using UnityEngine;
using System.Collections;

namespace BaseSystems.EventSystem
{
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