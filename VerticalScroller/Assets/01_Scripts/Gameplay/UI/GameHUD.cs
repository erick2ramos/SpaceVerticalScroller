using UnityEngine;
using System.Collections;
using BaseSystems.EventSystem;
using BaseSystems.Managers;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GameplayLogic.UI
{

    /// <summary>
    /// Updates the score, highscore and player continues left
    /// </summary>
    public class GameHUD : MonoBehaviour, IEventListener<GenericEvent>, IEventListener<ScoreUpdateEvent>
    {
        [SerializeField]
        TextMeshProUGUI _highScoreText;
        [SerializeField]
        TextMeshProUGUI _currenScoreText;
        [SerializeField]
        GameObject IconToCopy;
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
                case GenericEventType.LevelStarted:
                    // Set up and instantiate copies of the continue image
                    _continues = new Image[_gameManager.MaxLives];
                    _continues[0] = IconToCopy.GetComponent<Image>();
                    for(int i = 1; i < _gameManager.MaxLives; i++)
                    {
                        _continues[i] = Instantiate(IconToCopy, IconToCopy.transform.parent, false).GetComponent<Image>();
                    }
                    break;
                case GenericEventType.PlayerDied:
                    // show the amount of continues left
                    for(var i = 0; i < _continues.Length; i++)
                    {
                        _continues[i].gameObject.SetActive(i < _gameManager.CurrentLifes);
                    }
                    break;
            }
        }

        public void OnEvent(ScoreUpdateEvent eventType)
        {
            // Update the score ui texts
            _currenScoreText.text = eventType.NewScore.ToString("n0");
            _highScoreText.text = _gameManager.HighScore.ToString("n0");
        }

        private void OnEnable()
        {
            this.EventStartListening<GenericEvent>();
            this.EventStartListening<ScoreUpdateEvent>();
        }
        
        private void OnDisable()
        {
            this.EventStopListening<GenericEvent>();
            this.EventStopListening<ScoreUpdateEvent>();
        }
    }
}