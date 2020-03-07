using UnityEngine;
using System.Collections;
using BaseSystems.Managers;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using BaseSystems.Generic;

namespace GameplayLogic
{
    [System.Serializable]
    internal class EnemyTypeReferenceMapper
    {
        public EnemyType Type;
        public AssetReference Reference;
    }

    public class SpawnerManager : Manager
    {
        [SerializeField]
        EnemyTypeReferenceMapper[] _enemyAssetMapper;
        Dictionary<EnemyType, AssetReference> _enemyAssetTable = new Dictionary<EnemyType, AssetReference>();
        Dictionary<EnemyType, DynamicObjectPool> _enemyPools = new Dictionary<EnemyType, DynamicObjectPool>();
        Dictionary<string, SpawnerHandler> _registeredSpawners = new Dictionary<string, SpawnerHandler>();

        public override void Initialize()
        {
            base.Initialize();
            for(int i = 0; i < _enemyAssetMapper.Length; i++)
            {
                _enemyAssetTable[_enemyAssetMapper[i].Type] = _enemyAssetMapper[i].Reference;
            }
        }

        public void CreateEnemyPools()
        {
            for (int i = 0; i < _enemyAssetMapper.Length; i++)
            {
                var currentMap = _enemyAssetMapper[i];
                var handle = _enemyAssetMapper[i].Reference.LoadAssetAsync<GameObject>();
                handle.Completed += (h) =>
                {
                    // Creating enemy 
                    _enemyPools[currentMap.Type] = new DynamicObjectPool()
                    {
                        GameObjectPrefab = h.Result
                    };

                    _enemyPools[currentMap.Type].Create();
                };
            }
        }

        public void RegisterSpawner(SpawnerHandler handler)
        {
            _registeredSpawners[handler.Id] = handler;
        }

        public void UnregisterSpawner(SpawnerHandler handler)
        {
            _registeredSpawners.Remove(handler.Id);
        }

        public void SpawnEnemy(string id, EnemyType type)
        {
            SpawnerHandler handler;

            if(_registeredSpawners.TryGetValue(id, out handler))
            {
                DynamicObjectPool pool;
                if (_enemyPools.TryGetValue(type, out pool))
                {
                    handler.Spawn(pool.Get());
                }
            } else
            {
                throw new System.Exception($"Error: {id} spawner not registered");
            }
        }
    }
}