using UnityEngine;
using System.Collections;

namespace GameplayLogic.AI
{
    /// <summary>
    /// The AI focus on the target for LookupTime and then launches itself
    /// in last calculated direction
    /// </summary>
    public class AIBehaviourKamikaze : AIBehaviour
    {
        public string TransitionToState;
        public float LookupTime;
        public float LaunchTime;
        public float LaunchSpeed;
        float _lookupTimer = 0;

        Vector3 _launchDirection;
        Character _character;
        bool _active = true;
        Quaternion _originalRotation;

        public override void Initialize()
        {
            base.Initialize();
            _character = GetComponentInChildren<Character>();
            _originalRotation = _character.CharacterModel.localRotation;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _active = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            _lookupTimer = 0;
            _character.CharacterModel.transform.localRotation = _originalRotation;
        }

        public override void ProcessBehaviour()
        {
            if (!_active)
                return;

            if(_brain.Target != null)
            {
                _lookupTimer += Time.deltaTime;
                if (_lookupTimer > LookupTime)
                {
                    Launch();
                } else
                {
                    Lookup();
                }
            } else
            {
                Launch();
            }
        }

        private void Lookup()
        {
            _launchDirection = (_brain.Target.position - transform.position);
            _character.CharacterModel.localRotation = 
                Quaternion.Euler(0,0, Mathf.Rad2Deg * Mathf.Atan2(_launchDirection.y, _launchDirection.x));
        }

        private void Launch()
        {
            transform.Translate(_launchDirection.normalized * LaunchSpeed * Time.deltaTime);

            if(_lookupTimer > (LookupTime + LaunchTime))
            {
                _active = false;
                _brain.TransitionToState(TransitionToState);
            }
        }
    }
}