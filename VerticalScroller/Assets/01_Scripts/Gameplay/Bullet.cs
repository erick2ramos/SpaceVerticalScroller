using UnityEngine;
using System.Collections;

namespace GameplayLogic
{
    public class Bullet : MonoBehaviour
    {
        public float Speed = 7;
        public float MaxLifeTime = 5;

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

        public void Fire(Vector3 position, Vector3 direction)
        {
            transform.position = position;
            _moveDirection = direction;
            _isPrimed = true;
            _lifeTimer = MaxLifeTime;
            gameObject.SetActive(true);
        }
    }
}