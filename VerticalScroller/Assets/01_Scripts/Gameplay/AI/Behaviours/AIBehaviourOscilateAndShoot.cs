using UnityEngine;
using System.Collections;

namespace GameplayLogic.AI
{

    /// <summary>
    /// AI does an oscilating circular movement and after a while transitions a target state
    /// </summary>
    public class AIBehaviourOscilateAndShoot : AIBehaviour
    {
        public Vector2 ShootTimeInterval;
        public string TransitionToState;
        float _timer = 0;
        float _cooldown;

        public override void OnEnter()
        {
            base.OnEnter();
            _cooldown = Random.Range(ShootTimeInterval.x, ShootTimeInterval.y);
            _timer = 0;
        }

        public override void ProcessBehaviour()
        {
            _timer += Time.deltaTime;
            if(_timer >= _cooldown)
            {
                _brain.TransitionToState(TransitionToState);
            }

            Vector3 newPos = transform.position;
            newPos.x += Mathf.Sin(Time.time) * Time.deltaTime;
            newPos.y += Mathf.Cos(Time.time) * Time.deltaTime;
            transform.position = newPos;
        }
    }
}