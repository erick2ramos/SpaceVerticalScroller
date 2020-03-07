using UnityEngine;
using System.Collections;

namespace GameplayLogic.AI
{
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