using UnityEngine;
using System.Collections;
using BaseSystems.EventSystem;
using GameplayLogic.Events;

namespace GameplayLogic.UI
{
    public class HealthBarHandler : MonoBehaviour, IEventListener<DamageTakenEvent>, IEventListener<GenericEvent>
    {
        [SerializeField]
        Transform _slider;
        [SerializeField]
        Renderer[] Renderers;

        Coroutine blinkCoroutine;

        public void Initialize(Health health)
        {
            gameObject.SetActive(health.MaxHealth > 0);
            _slider.localScale = Vector3.one;
        }

        public void OnEvent(DamageTakenEvent eventType)
        {
            if(eventType.DamagedActor.CharacterType == CharacterType.Player)
            {
                Health h = eventType.DamagedActor.GetComponent<Health>();
                float _normalizedHealth = eventType.CurrentHealth / (float) h.MaxHealth;
                Vector3 newScale = Vector3.one;
                newScale.x = _normalizedHealth;
                _slider.localScale = newScale;
                if(eventType.CurrentHealth == 1)
                {
                    blinkCoroutine = StartCoroutine(SetBlink());
                }
            }
        }

        public void OnEvent(GenericEvent eventType)
        {
            switch (eventType.EventType)
            {
                case GenericEventType.RespawnCompleted:
                    if(blinkCoroutine != null)
                        StopCoroutine(blinkCoroutine);
                    _slider.localScale = Vector3.one;
                    foreach (var rend in Renderers)
                    {
                        rend.enabled = true;
                    }
                    break;
            }
        }

        private void OnEnable()
        {
            this.EventStartListening<GenericEvent>();
            this.EventStartListening<DamageTakenEvent>();
        }

        private void OnDisable()
        {
            this.EventStopListening<GenericEvent>();
            this.EventStopListening<DamageTakenEvent>();
        }

        public IEnumerator SetBlink()
        {
            float _timer = 0;
            int frames = 0;
            while (true)
            {
                if (frames % 10 == 0)
                {
                    foreach(var rend in Renderers)
                    {
                        rend.enabled = !rend.enabled;
                    }
                }
                frames++;
                _timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}