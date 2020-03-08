using UnityEngine;
using System.Collections;
using BaseSystems.EventSystem;
using GameplayLogic.Events;
using GameplayLogic.UI;
using BaseSystems.Feedback;

namespace GameplayLogic
{
    public class Health : MonoBehaviour
    {
        public bool IsInvulnerable { get; set; }
        public int CurrentHealth { get { return _currentHealth; } }
        public int MaxHealth { get { return _maxHealth; } }

        public Renderer CharacterRenderer;
        public int ScoreOnDeath;

        [SerializeField]
        private int _maxHealth;
        [SerializeField]
        private HealthBarHandler _healthBarHandler;
        [SerializeField]
        private Feedbacks _deathFeedback;

        private int _currentHealth;
        Character _character;

        public void SetCharacter(Character character)
        {
            _character = character;
            if(_healthBarHandler != null)
                _healthBarHandler.Initialize(this);
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
                SetInvulnerable(true);
                StartCoroutine(SetDamageEnabledInTime(invulnerableTime));
            }

            if(blinkTime > 0)
            {
                StartCoroutine(SetBlink(blinkTime));
            }

            if (_currentHealth <= 0)
            {
                Kill();
            }
        }

        public void Kill()
        {
            _deathFeedback?.PlayAll();
            _currentHealth = 0;
            SetInvulnerable(false);
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

        public void SetInvulnerable(bool isInvulnerable)
        {
            IsInvulnerable = isInvulnerable;
        }

        // Enable damage after a while
        public IEnumerator SetDamageEnabledInTime(float time)
        {
            yield return new WaitForSeconds(time);

            IsInvulnerable = false;
        }

        // Show when the character is invulnerable
        public IEnumerator SetBlink(float time)
        {
            float _timer = 0;
            int frames = 0;
            while (_timer <= time)
            {
                if (frames % 10 == 0)
                {
                    CharacterRenderer.enabled = !CharacterRenderer.enabled;
                }
                frames++;
                _timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            CharacterRenderer.enabled = true;
        }
    }
}