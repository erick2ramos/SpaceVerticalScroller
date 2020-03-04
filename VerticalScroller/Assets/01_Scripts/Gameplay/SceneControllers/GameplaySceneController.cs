using UnityEngine;
using System.Collections;
using BaseSystems.SceneHandling;

namespace GameplayLogic
{
    public class GameplaySceneModel : SceneModel
    {

    }

    public class GameplaySceneController : SceneController<GameplaySceneModel>
    {
        public override IEnumerator Initialization()
        {

            // Create bullet pool
            // Instantiate enemy spawner
            // Instantiate player ship
            yield return null;
        }
    }
}