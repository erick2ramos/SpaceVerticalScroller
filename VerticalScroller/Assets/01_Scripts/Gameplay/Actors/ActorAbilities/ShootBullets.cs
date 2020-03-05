using UnityEngine;
using System.Collections;
using BaseSystems.Generic;

namespace GameplayLogic
{
    public class ShootBullets : CharacterAbility
    {
        public float ShootCooldown = 0.25f;
        // Bullet pool
        public DynamicObjectPool Magazine;

        float _cooldownTimer = 0;

        public override void Initialize()
        {
            base.Initialize();
            Magazine.Create();
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
            // Get an inactive bullet from the pool
            GameObject bulletObj = Magazine.Get();

            // Activate the bullet an shoot it
            bulletObj.GetComponent<Bullet>().Fire(transform.position, Vector3.up);
        }
    }
}