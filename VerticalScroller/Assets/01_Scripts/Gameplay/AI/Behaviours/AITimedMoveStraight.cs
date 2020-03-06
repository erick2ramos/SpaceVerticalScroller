using UnityEngine;
using System.Collections;

namespace GameplayLogic.AI
{
    public class AITimedMoveStraight : AIBehaviour
    {
        public Vector2 MovementDirection;
        public float Speed;
        public string TransitionToState;
        public float StateTime = 3f;

        float _timer;

        public override void Initialize()
        {
            base.Initialize();

        }

        public override void OnEnter()
        {
            base.OnEnter();
            _timer = 0;
        }

        public override void ProcessBehaviour()
        {
            transform.Translate(MovementDirection * Speed * Time.deltaTime);

            if(_timer > StateTime)
            {
                _brain.TransitionToState(TransitionToState);
            }

            _timer += Time.deltaTime;
        }
    }
}