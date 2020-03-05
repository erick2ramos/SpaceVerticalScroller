using UnityEngine;
using System.Collections;
using BaseSystems.Managers;
using BaseSystems.EventSystem;

namespace BaseSystems.InputSystem
{
    public class InputManager : Manager
    {
        Vector3 _mainMovement;
        public Vector3 MovementDirection { get { return _mainMovement; } }
        public bool FireButtonDown { get; private set; }
        public bool PauseButtonDown { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void Update()
        {
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");

            if (Input.GetButtonDown("Shoot"))
            {
                FireButtonDown = true;
            } else if (Input.GetButtonUp("Shoot"))
            {
                FireButtonDown = false;
            }

            if (Input.GetButtonDown("Pause"))
            {
                GenericEvent.Trigger(GenericEventType.Pause, null);
            }

            _mainMovement.x = horizontalMovement;
            // Using y axis because this is a 2D Unity game
            _mainMovement.y = verticalMovement;
        }
    }
}