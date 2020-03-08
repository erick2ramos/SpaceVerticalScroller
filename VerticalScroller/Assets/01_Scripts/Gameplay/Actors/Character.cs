using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameplayLogic.AI;
using BaseSystems.EventSystem;

namespace GameplayLogic
{
    public enum ConditionState
    {
        Normal,
        Paused,
        Dead
    }

    public enum CharacterType
    {
        Player,
        NPC
    }

    public class Character : MonoBehaviour, IEventListener<GenericEvent>
    {
        public CharacterType CharacterType;
        public Transform CharacterModel;

        private CharacterAbility[] _abilities;
        private ConditionState _condition;
        private Health _health;
        private AIBrain _brain;
        bool _init = false;

        private void Start()
        {
            if(!_init)
                Initialize();
        }

        public void Initialize()
        {
            _init = true;
            _health = GetComponent<Health>();
            if(_health != null)
            {
                _health.SetCharacter(this);
                _health.Revive();
            }
            _condition = ConditionState.Normal;
            _abilities = GetComponents<CharacterAbility>();
            _brain = GetComponent<AIBrain>();

            foreach(var ability in _abilities)
            {
                if(ability.IsPermitted && ability.isActiveAndEnabled)
                {
                    ability.Initialize();
                }
            }

            if(_brain != null)
            {
                _brain.Activate();
            }
        }

        // Respawn the player when it's dead at param point
        public void RespawnAt(Vector3 respawnPosition)
        {
            if (!_init)
                Initialize();
            transform.position = respawnPosition;

            if(CharacterType == CharacterType.NPC)
            {
                if(_brain != null)
                {
                    _brain.ResetBrain();
                }
            }

            // Reset health on respawn
            if(_health != null)
                _health.Revive();
        }

        public void RespawnAt(SpawnerHandler handler)
        {
            RespawnAt(handler.transform.position);
        }

        private void Update()
        {
            EarlyAbilitiesUpdate();
            AbilitiesUpdate();
            LateAbilitiesUpdate();
        }

        private void EarlyAbilitiesUpdate()
        {
            foreach(var ability in _abilities)
            {
                if(ability.IsPermitted && ability.isActiveAndEnabled)
                {
                    ability.EarlyProcessAbility();
                }
            }
        }

        private void AbilitiesUpdate()
        {
            foreach (var ability in _abilities)
            {
                if (ability.IsPermitted && ability.isActiveAndEnabled)
                {
                    ability.ProcessAbility();
                }
            }
        }

        private void LateAbilitiesUpdate()
        {
            foreach (var ability in _abilities)
            {
                if (ability.IsPermitted && ability.isActiveAndEnabled)
                {
                    ability.LateProcessAbility();
                }
            }
        }

        public void OnEvent(GenericEvent eventType)
        {
            switch (eventType.EventType)
            {
                case GenericEventType.GameOver:
                case GenericEventType.LevelCompleted:
                    enabled = false;
                    if (_brain != null)
                        _brain.IsActive = false;
                    if (_health != null)
                        _health.SetInvulnerable(true);
                    break;
            }
        }

        private void OnEnable()
        {
            this.EventStartListening();
        }

        private void OnDisable()
        {
            this.EventStopListening();
        }
    }
}