using UnityEngine;
using System.Collections;
using BaseSystems.Generic;
using BaseSystems.Managers;

namespace GameplayLogic
{
    public class ShootBullets : CharacterAbility
    {
        public float ShootCooldown = 0.25f;
        public BulletType BulletType;
        float _cooldownTimer = 0;

        SpawnerManager _spawnerManager;

        public override void Initialize()
        {
            base.Initialize();
            _spawnerManager = ManagerProvider.Get<SpawnerManager>();
        }

        public override void EarlyProcessAbility()
        {
            base.EarlyProcessAbility();

            if (_cooldownTimer > 0)
                _cooldownTimer -= Time.deltaTime;
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();

            if (_input.FireButtonDown && _cooldownTimer <= 0)
            {
                _cooldownTimer = ShootCooldown;
                Shoot();
            }
        }

        protected virtual void Shoot()
        {
            // Get an inactive bullet from the pool and shoot it
            _spawnerManager.LendBullet(BulletType).Fire(transform.position, Vector3.up, gameObject);
        }
    }
}