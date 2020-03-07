using UnityEngine;
using System.Collections;

namespace BaseSystems.EventSystem
{
    public struct ScoreUpdateEvent
    {
        public int NewScore;

        public ScoreUpdateEvent(int newScore)
        {
            NewScore = newScore;
        }

        static ScoreUpdateEvent e;

        public static void Trigger(int newScore)
        {
            e.NewScore = newScore;
            EventManager.Trigger(e);
        }
    }
}