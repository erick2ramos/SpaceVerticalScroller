using UnityEngine;
using System.Collections;
using BaseSystems.SceneHandling;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using BaseSystems.Generic;
using BaseSystems.EventSystem;
using BaseSystems.Managers;

namespace GameplayLogic
{
    public class GameplaySceneModel : SceneModel
    {

    }

    public class GameplaySceneController : SceneController<GameplaySceneModel>, IEventListener<GenericEvent>
    {
        [SerializeField]
        private Transform _playerHolder;
        [SerializeField]
        private AssetReference _playerReference;
        [SerializeField]
        private WaveSequence _waveSequence;
        private SpawnerManager _spawnerManager;
        private GameManager _gameManager;

        public override IEnumerator Initialization()
        {
            _spawnerManager = ManagerProvider.Get<SpawnerManager>();
            _gameManager = ManagerProvider.Get<GameManager>();

            _spawnerManager.CreateEnemyPools();
            _gameManager.SetupLevel(3);

            // Instantiate player ship
            AsyncOperationHandle<GameObject> handle = _playerReference.InstantiateAsync(_playerHolder, false);

            _waveSequence.Initialize();

            yield return new WaitForSeconds(1);

            GenericEvent.Trigger(GenericEventType.LevelStarted, null);
        }

        public void OnEvent(GenericEvent eventType)
        {
            switch (eventType.EventType)
            {
                case GenericEventType.LevelStarted:
                    _waveSequence.Activate();
                    break;
                case GenericEventType.GameOver:
                    _waveSequence.Deactivate();
                    // Show game over screen
                    break;
            }
        }

        private void OnEnable()
        {
            this.EventStartListening();
        }

        private void OnDisable()
        {
            this.EventStopListening();
        }
    }
}