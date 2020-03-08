using UnityEngine;
using System.Collections;

namespace BaseSystems.DataPersistance
{
    // Used to store player data to save in a binary file
    // Any field that needs to be saved can be added to this class
    [System.Serializable]
    public class PlayerData
    {
        public int CurrentHighScore;
        
        public PlayerData()
        {
            
        }
    }
}