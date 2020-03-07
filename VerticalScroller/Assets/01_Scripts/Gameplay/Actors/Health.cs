using UnityEngine;
using System.Collections;
using BaseSystems.EventSystem;
using GameplayLogic.Events;

namespace GameplayLogic
{
    public class Health : MonoBehaviour
    {
        public bool IsInvulnerable { get; set; }
        public int CurrentHealth { get { return _currentHealth; } }

        public int ScoreOnDeath;

        [SerializeField]
        private int _maxHealth;
        private int _currentHealth;
        Character _character;

        public void SetCharacter(Character character)
        {
            _character = character;
        }

        public void Damage(int amount, float blinkTime = 0, float invulnerableTime = 0)
        {
            // Don't do anything if we're already below 0
            if (_currentHealth <= 0 || IsInvulnerable)
                return;

            var previousHealth = _currentHealth;
            _currentHealth -= amount;
            _currentHealth = Mathf.Max(0, _currentHealth);

            DamageTakenEvent.Trigger(amount, previousHealth, _currentHealth, _character);

            if (invulnerableTime > 0)
            {
                SetDamageable(false);
                StartCoroutine(SetDamageEnabledInTime(invulnerableTime));
            }

            if (_currentHealth <= 0)
            {
                Kill();
            }
        }

        public void Kill()
        {
            _currentHealth = 0;
            SetDamageable(false);
            gameObject.SetActive(false);
            if (_character.CharacterType == CharacterType.Player)
                GenericEvent.Trigger(GenericEventType.PlayerDied, gameObject);
            else
                GenericEvent.Trigger(GenericEventType.EnemyDestroyed, gameObject);
        }

        public void Revive()
        {
            _currentHealth = _maxHealth;
            gameObject.SetActive(true);
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