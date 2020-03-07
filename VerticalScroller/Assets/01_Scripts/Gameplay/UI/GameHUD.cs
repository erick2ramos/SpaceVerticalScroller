using UnityEngine;
using System.Collections;
using BaseSystems.EventSystem;
using BaseSystems.Managers;
using TMPro;
using UnityEngine.UI;

namespace GameplayLogic.UI
{
    public class GameHUD : MonoBehaviour, IEventListener<GenericEvent>
    {
        [SerializeField]
        TextMeshProUGUI _highScoreText;
        [SerializeField]
        TextMeshProUGUI _currenScoreText;
        [SerializeField]
        Image[] _continues;

        GameManager _gameManager;

        private void Awake()
        {
            _gameManager = ManagerProvider.Get<GameManager>();
            _highScoreText.text = _gameManager.HighScore.ToString("n0");
        }

        public void OnEvent(GenericEvent eventType)
        {
            switch (eventType.EventType)
            {
                case GenericEventType.PlayerDied:
                    // show the amount of continues
                    for(var i = 0; i < _continues.Length; i++)
                    {
                        _continues[i].gameObject.SetActive(i < _gameManager.CurrentLives);
                    }
                    break;
                case GenericEventType.EnemyDestroyed:
                    // change the current score
                    _currenScoreText.text = _gameManager.CurrentScore.ToString("n0");
                    break;
            }
        }

        private void OnEnable()
        {
            this.EventStartListening<GenericEvent>();
        }
        
        private void OnDisable()
        {
            this.EventStopListening<GenericEvent>();
        }
    }
}