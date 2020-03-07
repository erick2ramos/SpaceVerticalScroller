using UnityEngine;
using System.Collections;

namespace BaseSystems.EventSystem
{
    public enum GenericEventType
    {
        LevelLoaded,
        LevelStarted,
        LevelCompleted,
        LevelEnd,
        PlayerDied,
        GameOver,
        Pause,
        RespawnStarted,
        RespawnCompleted,
        EnemyDestroyed
    }

    public struct GenericEvent
    {
        public GenericEventType EventType;
        public GameObject Originator;

        public GenericEvent(GenericEventType eventName, GameObject originator)
        {
            EventType = eventName;
            Originator = originator;
        }

        static GenericEvent e;

        public static void Trigger(GenericEventType eventName, GameObject originator)
        {
            e.EventType = eventName;
            e.Originator = originator;
            EventManager.Trigger(e);
        }
    }
}