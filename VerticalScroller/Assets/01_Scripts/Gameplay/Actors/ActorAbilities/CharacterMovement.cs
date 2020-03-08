using UnityEngine;
using System.Collections;

namespace GameplayLogic
{
    public class CharacterMovement : CharacterAbility
    {
        public float MaxSpeed = 2;
        public Rect GameplayArea;
        protected Vector3 Velocity;

        public override void Initialize()
        {
            base.Initialize();

            BoxCollider2D b2d = GetComponent<BoxCollider2D>();

            if(Camera.main != null)
            {
                float height = Camera.main.orthographicSize;
                float width = height * Camera.main.aspect;

                GameplayArea.x = -width + (b2d.size.x * 0.5f);
                GameplayArea.y = -height + (b2d.size.y * 0.5f);

                GameplayArea.width = (width - (b2d.size.x * 0.5f)) * 2;
                GameplayArea.height = (height - (b2d.size.y * 0.5f)) * 2;
                
            }
        }

        protected override void ProcessInput()
        {
            Velocity = _input.MovementDirection * MaxSpeed * Time.deltaTime;

            if (!GameplayArea.Contains(transform.position + Velocity))
            {
                return;
            }

            transform.Translate(Velocity);

        }
    }
}