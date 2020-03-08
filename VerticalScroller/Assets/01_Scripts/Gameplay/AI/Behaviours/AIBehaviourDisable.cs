using UnityEngine;
using System.Collections;

namespace GameplayLogic.AI
{
    /// <summary>
    /// Disables the game object, useful for the DynamicObjectPool
    /// </summary>
    public class AIBehaviourDisable : AIBehaviour
    {
        public override void ProcessBehaviour()
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();
            _brain.gameObject.SetActive(false);
            _brain.ResetBrain();
        }
    }
}