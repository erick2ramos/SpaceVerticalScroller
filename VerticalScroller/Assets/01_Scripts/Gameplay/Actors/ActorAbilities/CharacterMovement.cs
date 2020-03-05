using UnityEngine;
using System.Collections;

namespace GameplayLogic
{
    public class CharacterMovement : CharacterAbility
    {
        public float MaxSpeed = 2;
        protected Vector3 Velocity;

        protected override void ProcessInput()
        {
            Velocity = _input.MovementDirection * MaxSpeed * Time.deltaTime;

            transform.Translate(Velocity);
        }
    }
}