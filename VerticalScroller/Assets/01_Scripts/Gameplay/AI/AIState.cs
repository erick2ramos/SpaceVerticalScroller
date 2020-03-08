using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameplayLogic.AI
{

    /// <summary>
    /// State in charge of processing all the registered behaviours
    /// </summary>
    [System.Serializable]
    public class AIState
    {
        public string StateID;
        public List<AIBehaviour> Behaviours;

        public void Initialize()
        {
            foreach (var behaviour in Behaviours)
            {
                behaviour.Initialize();
            }
        }

        public void OnEnter()
        {
            foreach (var behaviour in Behaviours)
            {
                behaviour.OnEnter();
            }
        }

        public void ProcessState()
        {
            foreach (var behaviour in Behaviours)
            {
                behaviour.ProcessBehaviour();
            }
        }

        public void OnExit()
        {
            foreach (var behaviour in Behaviours)
            {
                behaviour.OnExit();
            }
        }
    }
}