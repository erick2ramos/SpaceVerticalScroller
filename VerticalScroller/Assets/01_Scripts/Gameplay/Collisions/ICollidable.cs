using UnityEngine;
using System.Collections;

namespace GameplayLogic
{
    public interface ICollidable
    {
        void ProcessCollision(CollisionInfo info);
    }

    public struct CollisionInfo
    {
        public int AmountOfDamage;
        public Character Agressor;
    }
}