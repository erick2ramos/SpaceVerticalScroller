using UnityEngine;
using System.Collections;
using BaseSystems.InputSystem;
using BaseSystems.Managers;

namespace GameplayLogic
{
    public class CharacterAbility : MonoBehaviour
    {
        public bool IsPermitted = true;
        protected Character _character;
        protected Health _health;
        protected InputManager _input;

        public virtual void Initialize()
        {
            _character = GetComponent<Character>();
            _health = GetComponent<Health>();
            _input = ManagerProvider.Get<InputManager>();
        }

        protected virtual void ProcessInput()
        {

        }

        // Would be called as an "EarlyUpdate"
        public virtual void EarlyProcessAbility() 
        {
            ProcessInput();
        }

        // Would be called as an Update
        public virtual void ProcessAbility() { }
        
        // Would be called as an LateUpdate
        public virtual void LateProcessAbility() { }

        public virtual void ResetAbility()
        {

        }
    }
}