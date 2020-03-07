using UnityEngine;
using System.Collections;
using BaseSystems.SceneHandling;
using BaseSystems.Managers;
using TMPro;

namespace GameplayLogic
{
    public class GameOverSceneModel : SceneModel
    {
        public int CurrentScore;
        public int HighScore;

        public System.Action OnAccept;
    }

    public class GameOverSceneController : SceneController<GameOverSceneModel>
    {
        [SerializeField]
        TextMeshProUGUI _currentScoreText;
        [SerializeField]
        TextMeshProUGUI _highScoreText;

        bool _accept = false;
        bool _init = false;

        SceneTransitionManager _transitionManager;

        public override IEnumerator Initialization()
        {
            _currentScoreText.text = Model.CurrentScore.ToString("n0");
            _highScoreText.text = Model.HighScore.ToString("n0");

            _transitionManager = ManagerProvider.Get<SceneTransitionManager>();
            yield return new WaitForSeconds(3);
            _init = true;
        }

        private void Update()
        {
            // wait for any input
            if (_init && !_accept && Input.anyKey)
            {
                _accept = true;
                Model.OnAccept?.Invoke();
                _transitionManager.CloseAdditiveScene(this);
            }
        }
    }
}