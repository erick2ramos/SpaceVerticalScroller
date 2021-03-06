﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameplayLogic.AI
{
    /// <summary>
    /// Process the AI behaviours configured in the character
    /// </summary>
    public class AIBrain : MonoBehaviour
    {
        [System.NonSerialized]
        public Character Character;
        [System.NonSerialized]
        public Transform Target;
        public bool IsActive = false;

        // AI for enemies
        // State Machine for customizable enemy behaviours
        [SerializeField]
        List<AIState> _states;
        AIState _currentState;
        public string CurrentState;

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
            // Target look up
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            Target = go != null ? go.transform : null;
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
                CurrentState = _currentState.StateID;

                if (_currentState != null)
                {
                    _currentState.OnEnter();
                }
            }
        }

        public void ResetBrain()
        {
            TransitionToState(_states[0].StateID);
        }
    }
}