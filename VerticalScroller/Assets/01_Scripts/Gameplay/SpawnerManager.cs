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

    [System.Serializable]
    internal class BulletTypeReferenceMapper
    {
        public BulletType Type;
        public AssetReference Reference;
    }

    public class SpawnerManager : Manager
    {
        [SerializeField]
        EnemyTypeReferenceMapper[] _enemyAssetMapper;
        [SerializeField]
        BulletTypeReferenceMapper[] _bulletAssetMapper;

        Dictionary<EnemyType, AssetReference> _enemyAssetTable = new Dictionary<EnemyType, AssetReference>();
        Dictionary<BulletType, AssetReference> _bulletAssetTable = new Dictionary<BulletType, AssetReference>();
        Dictionary<EnemyType, DynamicObjectPool> _enemyPools = new Dictionary<EnemyType, DynamicObjectPool>();
        Dictionary<BulletType, DynamicObjectPool> _bulletPools = new Dictionary<BulletType, DynamicObjectPool>();
        Dictionary<string, SpawnerHandler> _registeredSpawners = new Dictionary<string, SpawnerHandler>();

        public override void Initialize()
        {
            base.Initialize();
            for(int i = 0; i < _enemyAssetMapper.Length; i++)
            {
                _enemyAssetTable[_enemyAssetMapper[i].Type] = _enemyAssetMapper[i].Reference;
            }
            for(int i = 0; i < _bulletAssetMapper.Length; i++)
            {
                _bulletAssetTable[_bulletAssetMapper[i].Type] = _bulletAssetMapper[i].Reference;
            }
        }

        public void CreateEnemyPools()
        {
            _enemyPools.Clear();
            _bulletPools.Clear();

            for (int i = 0; i < _enemyAssetMapper.Length; i++)
            {
                var currentMap = _enemyAssetMapper[i];
                // Preload a pool of each enemy to reuse them
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

            for (int i = 0; i < _bulletAssetMapper.Length; i++)
            {
                // Preload a pool of each bullet type to reuse them
                BulletType type = _bulletAssetMapper[i].Type;
                var bHandle = _bulletAssetTable[_bulletAssetMapper[i].Type].LoadAssetAsync<GameObject>();
                bHandle.Completed += (h) =>
                {
                    _bulletPools[type] = new DynamicObjectPool()
                    {
                        GameObjectPrefab = h.Result
                    };
                    _bulletPools[type].Create();
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

        public SpawnerHandler GetSpawnerByID(string id)
        {
            SpawnerHandler handler;

            if (_registeredSpawners.TryGetValue(id, out handler))
            {
                return handler;
            }
            else
            {
                throw new System.Exception($"Error: {id} spawner not registered");
            }
        }

        /// <summary>
        /// Returns a bullet of BulletType from the matching DynamicObjectPool
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Bullet LendBullet(BulletType type)
        {
            DynamicObjectPool pool;

            if(_bulletPools.TryGetValue(type, out pool))
            {
                return pool.Get().GetComponent<Bullet>();
            } else
            {
                throw new System.Exception($"Error: {type} bullet type is not configured");
            }
        }
    }
}