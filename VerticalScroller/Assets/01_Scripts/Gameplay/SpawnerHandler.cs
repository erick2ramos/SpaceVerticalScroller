using UnityEngine;
using System.Collections;
using BaseSystems.Managers;

namespace GameplayLogic
{

    public class SpawnerHandler : MonoBehaviour
    {
        public string Id;
        SpawnerManager _spawnerManager;

        private void OnEnable()
        {
            _spawnerManager = ManagerProvider.Get<SpawnerManager>();
            _spawnerManager.RegisterSpawner(this);
        }

        private void OnDisable()
        {
            _spawnerManager.UnregisterSpawner(this);
        }

        public void Spawn(GameObject objToSpawn)
        {
            Character character = objToSpawn.GetComponent<Character>();
            if (character != null)
            {
                character.RespawnAt(transform.position);
            } else
            {
                objToSpawn.transform.position = transform.position;
                objToSpawn.SetActive(true);
            }
        }
    }
}