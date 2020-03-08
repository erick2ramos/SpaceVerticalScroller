using UnityEngine;
using System.Collections;
using BaseSystems.Managers;
using BaseSystems.Feedback;

namespace GameplayLogic.AI
{

    /// <summary>
    /// AI shoots a configurable speed bullet at the target
    /// </summary>
    public class AIBehaviourShoot : AIBehaviour
    {
        public BulletType BulletType;
        public string TransitionToState;
        public float BulletSpeed;
        public float TransitionExitTime;

        [SerializeField]
        Feedbacks _shootingFeedback;

        SpawnerManager _spawnerManager;
        float _timer;

        public override void Initialize()
        {
            base.Initialize();
            _spawnerManager = ManagerProvider.Get<SpawnerManager>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Vector3 target = _brain.Target != null ? _brain.Target.position : Vector3.down;

            // Aim at target
            Vector3 direction = target - transform.position;

            var bullet = _spawnerManager.LendBullet(BulletType);
            // Assign bullet speed
            bullet.Speed = BulletSpeed;
            // Shoot a bullet at target
            bullet.Fire(transform.position, direction.normalized, gameObject);
            _timer = TransitionExitTime;
            _shootingFeedback.PlayAll();
        }

        public override void ProcessBehaviour()
        {
            if(_timer <= 0)
            {
                _brain.TransitionToState(TransitionToState);
            } else
            {
                _timer -= Time.deltaTime;
            }
        }
    }
}