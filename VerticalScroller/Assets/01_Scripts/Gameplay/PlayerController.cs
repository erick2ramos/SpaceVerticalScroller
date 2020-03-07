using UnityEngine;
using System.Collections;

namespace GameplayLogic
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        LayerMask _enemyCollisionMask;
        [SerializeField]
        LayerMask _bulletCollisionMask;

        Character _character;
        Health _health;

        private void Awake()
        {
            _character = GetComponent<Character>();
            _health = GetComponent<Health>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if((1 << collision.gameObject.layer & _enemyCollisionMask) > 0)
            {
                // Player collided with an enemy
                _health.Damage(CollisionHandler.COLLISION_DAMAGE_AMOUNT, invulnerableTime: 2.5f);
            }

            if((1 << collision.gameObject.layer & _bulletCollisionMask) > 0)
            {
                _health.Damage(1, invulnerableTime: 2.5f);
            }
        }
    }
}