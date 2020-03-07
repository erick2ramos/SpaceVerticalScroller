using UnityEngine;
using System.Collections;

namespace GameplayLogic
{
    [CreateAssetMenu(fileName = "WaveSequence", menuName = "Wave/Sequence")]
    public class WaveSequenceConfig : ScriptableObject
    {
        public Wave[] Waves;
    }
}