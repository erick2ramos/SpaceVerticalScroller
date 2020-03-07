using UnityEngine;
using System.Collections;

namespace GameplayLogic
{
    public enum BulletType
    {
        Normal,
        EnemyNormal,
        EnemySlow
    }

    public class Bullet : MonoBehaviour
    {
        public float Speed = 7;
        public float MaxLifeTime = 5;
        [System.NonSerialized]
        public GameObject Origin;

        // 5 seconds to make sure turn off the bullet
        float _lifeTimer = 5;
        Vector3 _moveDirection;
        bool _isPrimed = false;

        private void Update()
        {
            if (_isPrimed)
            {
                // Move bullet
                transform.Translate(_moveDirection * Speed * Time.deltaTime);

                _lifeTimer -= Time.deltaTime;
                if (_lifeTimer <= 0)
                    gameObject.SetActive(false);
            }
        }

        public void Fire(Vector3 position, Vector3 direction, GameObject origin)
        {
            Origin = origin;
            transform.position = position;
            _moveDirection = direction;
            _isPrimed = true;
            _lifeTimer = MaxLifeTime;
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var other = collision.gameObject.GetComponent<ICollidable>();
            if (other != null && Origin != null && Origin != collision.gameObject)
            {
                other.ProcessCollision(new CollisionInfo() 
                { 
                    AmountOfDamage = 1,
                    Agressor = Origin.GetComponent<Character>()
                });
                gameObject.SetActive(false);
            }
        }
    }
}