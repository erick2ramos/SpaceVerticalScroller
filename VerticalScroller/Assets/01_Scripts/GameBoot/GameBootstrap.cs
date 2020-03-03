using BaseSystems.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    ManagerProvider provider;

    IEnumerator Start()
    {
        provider = FindObjectOfType<ManagerProvider>();

        if(provider == null)
        {
            throw new System.Exception("Manager provider wasn't found in the initialization scene");
        }

        provider.Init();

        yield return null;
    }
}
