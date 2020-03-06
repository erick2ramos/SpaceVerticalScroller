using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BaseSystems.Generic
{
    [System.Serializable]
    public class DynamicObjectPool
    {
        public int MinAmount = 10;
        public GameObject GameObjectPrefab;
        List<GameObject> _pool;
        GameObject _parent;
        int currentIndex;

        public void Create()
        {
            _pool = new List<GameObject>();
            _parent = new GameObject($"__Pool__{GameObjectPrefab.name}");
            for (int i = 0; i < MinAmount; i++)
            {
                _pool.Add(Generate());
            }
        }

        public GameObject Generate()
        {
            var go = GameObject.Instantiate(GameObjectPrefab, _parent.transform, false);
            go.SetActive(false);
            return go;
        }

        public GameObject Get()
        {
            int startingIndex = currentIndex;
            while (_pool[currentIndex].activeSelf) 
            { 
                currentIndex = (currentIndex + 1) % _pool.Count;
                if(startingIndex == currentIndex)
                {
                    // there's no objects available
                    var newGO = Generate();
                    _pool.Add(newGO);
                }
            }
            return _pool[currentIndex];
        }
    }
}