using UnityEngine;
using System.Collections;
using BaseSystems.SceneHandling;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using BaseSystems.Generic;

namespace GameplayLogic
{
    public class GameplaySceneModel : SceneModel
    {

    }

    public class GameplaySceneController : SceneController<GameplaySceneModel>
    {
        [SerializeField]
        private Transform _enemiesHolder;
        [SerializeField]
        private Transform _playerHolder;
        [SerializeField]
        private Transform _bulletsHolder;
        [SerializeField]
        private AssetReference _playerReference;
        [SerializeField]
        // Only one type of enemy
        private AssetReference _enemyReference;
        DynamicObjectPool _enemyPool;
        float _timer = 0;

        public override IEnumerator Initialization()
        {
            // Instantiate enemy spawner
            // Instantiate player ship
            AsyncOperationHandle<GameObject> handle = _playerReference.InstantiateAsync(_playerHolder, false);
            handle.Completed += OnPlayerInstantiated;
            _enemyPool = new DynamicObjectPool();
            AsyncOperationHandle<GameObject> enemyHandle = _enemyReference.LoadAssetAsync<GameObject>();
            enemyHandle.Completed += OnEnemyAssetLoaded;

            yield return null;
        }

        public void OnPlayerInstantiated(AsyncOperationHandle<GameObject> handle)
        {

        }

        public void OnEnemyAssetLoaded(AsyncOperationHandle<GameObject> handle)
        {
            _enemyPool.GameObjectPrefab = handle.Result;
            _enemyPool.Create();
        }

        private void Update()
        {
            if(_timer > 4)
            {
                var enemy = _enemyPool.Get();
                Vector3 randomPos = new Vector3(Random.Range(-10, 10), Random.Range(12, 14), 0);
                enemy.transform.position = randomPos;
                enemy.SetActive(true);
                _timer = 0;
            }
            _timer += Time.deltaTime;
        }
    }
}