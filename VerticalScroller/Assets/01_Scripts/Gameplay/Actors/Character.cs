using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public class Character : MonoBehaviour
    {
        public CharacterType CharacterType;

        private CharacterAbility[] _abilities;
        private ConditionState _condition;
        private Health _health;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _health = GetComponent<Health>();
            _condition = ConditionState.Normal;
            _abilities = GetComponents<CharacterAbility>();

            foreach(var ability in _abilities)
            {
                if(ability.IsPermitted && ability.isActiveAndEnabled)
                {
                    ability.Initialize();
                }
            }
        }

        // Respawn the player when it's dead at param point
        public void RespawnAt(Vector3 respawnPosition)
        {
            // Reset health on respawn
            if(_health != null)
                _health.Revive();

            transform.position = respawnPosition;
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
    }
}