using UnityEngine;
using System.Collections;

namespace GameplayLogic
{
    public enum WinCondition
    {
        None = 0,
        Score = 1,
        WaveSurvived = 2,
    }

    /// <summary>
    /// Win condition config
    /// </summary>
    [CreateAssetMenu(fileName = "Game Config", menuName = "Game Config")]
    public class GameGlobalConfig : ScriptableObject
    {
        public WinCondition WinCondition;
        public int AmountToWin;
    }
}