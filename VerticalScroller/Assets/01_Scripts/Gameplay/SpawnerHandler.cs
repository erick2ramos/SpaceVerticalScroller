using UnityEngine;
using System.Collections;
using BaseSystems.Managers;

namespace GameplayLogic
{

    public class SpawnerHandler : MonoBehaviour
    {
        public string Id;
        SpawnerManager _spawnerManager;

        private void Awake()
        {
            _spawnerManager = ManagerProvider.Get<SpawnerManager>();
            _spawnerManager.RegisterSpawner(this);
        }

        public void Spawn(GameObject objToSpawn)
        {
            objToSpawn.transform.position = transform.position;
            objToSpawn.SetActive(true);
        }
    }
}