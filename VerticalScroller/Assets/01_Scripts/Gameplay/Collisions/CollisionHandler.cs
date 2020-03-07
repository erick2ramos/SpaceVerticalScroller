using UnityEngine;
using System.Collections;

namespace GameplayLogic
{
    [RequireComponent(typeof(Character), typeof(Health))]
    public class CollisionHandler : MonoBehaviour, ICollidable
    {
        public const int COLLISION_DAMAGE_AMOUNT = 1;

        Character _character;
        Health _health;

        protected virtual void Awake()
        {
            _character = GetComponent<Character>();
            _health = GetComponent<Health>();
        }

        public virtual void ProcessCollision(CollisionInfo info)
        {
            _health.Damage(info.AmountOfDamage);
        }
    }
}