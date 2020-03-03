using UnityEngine;
using System.Collections;

namespace BaseSystems.DataPersistance
{
    // Used to store player data to save in a binary file
    [System.Serializable]
    public class PlayerData
    {
        public int CurrentHighScore;
        
        public PlayerData()
        {
            
        }
    }
}