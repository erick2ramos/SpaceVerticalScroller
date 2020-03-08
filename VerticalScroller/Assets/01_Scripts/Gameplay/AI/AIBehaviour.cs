using UnityEngine;
using System.Collections;

namespace GameplayLogic.AI
{

    /// <summary>
    /// Custom behaviour for an npc character
    /// </summary>
    [RequireComponent(typeof(AIBrain))]
    public abstract class AIBehaviour : MonoBehaviour
    {
        protected AIBrain _brain;

        public virtual void Initialize()
        {
            _brain = GetComponent<AIBrain>();
        }

        public bool IsBehaviourDone { get; set; }
        
        public virtual void OnEnter() 
        {
            IsBehaviourDone = false;
        }

        public abstract void ProcessBehaviour();

        public virtual void OnExit()
        {
            IsBehaviourDone = true;
        }
    }
}