using UnityEngine;
using System.Collections;
using BaseSystems.EventSystem;

namespace GameplayLogic
{
    public class Health : MonoBehaviour
    {
        public bool IsInvulnerable { get; set; }
        public int CurrentHealth { get { return _currentHealth; } }

        [SerializeField]
        private int _maxHealth;
        private int _currentHealth;

        public void Damage(int amount, float blinkTime = 0, float invulnerableTime = 0)
        {
            // Don't do anything if we're already below 0
            if (_currentHealth <= 0 || IsInvulnerable)
                return;

            var previousHealth = _currentHealth;
            _currentHealth -= amount;
            _currentHealth = Mathf.Max(0, _currentHealth);

            DamageTakenEvent.Trigger(amount, previousHealth, _currentHealth);

            if (invulnerableTime > 0)
            {
                SetDamageable(false);
                StartCoroutine(SetDamageEnabledInTime(invulnerableTime));
            }

            if (_currentHealth <= 0)
            {
                GenericEvent.Trigger(GenericEventType.PlayerDied, gameObject);
                Kill();
            }
        }

        public void Kill()
        {
            _currentHealth = 0;
            SetDamageable(false);
        }

        public void Revive()
        {
            _currentHealth = _maxHealth;
        }

        public void SetDamageable(bool isInvulnerable)
        {
            IsInvulnerable = isInvulnerable;
        }

        // Enable damage after a while
        public IEnumerator SetDamageEnabledInTime(float time)
        {
            yield return new WaitForSeconds(time);

            IsInvulnerable = false;
        }
    }
}