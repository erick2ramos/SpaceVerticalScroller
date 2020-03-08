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
        // Set the frame rate to have virtual no limit
        Application.targetFrameRate = 300;

        // Find the service provider
        provider = FindObjectOfType<ManagerProvider>();
        if(provider == null)
        {
            throw new System.Exception("Manager provider wasn't found in the initialization scene");
        }

        // initialize the service provider
        provider.Init();

        // Allow stabilization so we wait a second here
        yield return new WaitForSeconds(1);

        var transitionManager = ManagerProvider.Get<SceneTransitionManager>();
        GameplaySceneModel model = new GameplaySceneModel();
        // Transition to the gameplay scene
        transitionManager.LoadScene(SceneIndex.GameplayScene, model);
    }
}
