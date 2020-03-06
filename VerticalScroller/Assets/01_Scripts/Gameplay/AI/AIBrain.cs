using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameplayLogic.AI
{
    public class AIBrain : MonoBehaviour
    {
        [System.NonSerialized]
        public Character Character;
        [System.NonSerialized]
        public Transform Target;
        public bool IsActive = false;

        // Simple AI for enemies
        // Simple State Machine for customizable enemy behaviours
        [SerializeField]
        List<AIState> _states;
        int _currentBehaviour = 0;
        AIState _currentState;

        private void Awake()
        {
            Character = GetComponent<Character>();
        }

        public void Activate()
        {
            _currentState = _states[0];
            IsActive = true;
            foreach (var state in _states)
            {
                state.Initialize();
            }
            // TODO: Search target
        }

        private void Update()
        {
            if (!IsActive)
                return;

            if(_currentState != null)
            {
                // Run the behaviour for the current state in the state machine
                _currentState.ProcessState();
            }
        }

        // A behaviour should take decisions and transition to another state
        public void TransitionToState(string stateID)
        {
            if (_currentState.StateID != stateID)
            {
                var newState = _states.Find((x) => x.StateID == stateID);
                _currentState.OnExit();
                _currentState = newState;

                if(_currentState != null)
                {
                    _currentState.OnEnter();
                }
            }
        }
    }
}