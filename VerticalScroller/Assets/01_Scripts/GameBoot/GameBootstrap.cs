using BaseSystems.Managers;
using BaseSystems.SceneHandling;
using GameplayLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    ManagerProvider provider;

    IEnumerator Start()
    {
        Application.targetFrameRate = 300;
        provider = FindObjectOfType<ManagerProvider>();

        if(provider == null)
        {
            throw new System.Exception("Manager provider wasn't found in the initialization scene");
        }

        provider.Init();

        yield return new WaitForSeconds(1);

        var transitionManager = ManagerProvider.Get<SceneTransitionManager>();
        GameplaySceneModel model = new GameplaySceneModel();
        transitionManager.LoadScene(SceneIndex.GameplayScene, model);
    }
}
