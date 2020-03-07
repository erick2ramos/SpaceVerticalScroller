using UnityEngine;
using System.Collections;
using BaseSystems.SceneHandling;
using BaseSystems.Managers;

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
        bool _accept = false;
        bool _init = false;

        SceneTransitionManager _transitionManager;

        public override IEnumerator Initialization()
        {
            _transitionManager = ManagerProvider.Get<SceneTransitionManager>();
            yield return new WaitForSeconds(2);
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