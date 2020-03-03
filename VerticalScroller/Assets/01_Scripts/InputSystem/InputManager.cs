using UnityEngine;
using System.Collections;
using BaseSystems.Managers;

namespace BaseSystems.InputSystem
{
    public class InputManager : Manager
    {
        Vector3 _mainMovement;
        public Vector3 MovementDirection { get { return _mainMovement; } }
        public bool FireButtonDown { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void Update()
        {
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");

            if (Input.GetButtonDown("Fire1"))
            {
                FireButtonDown = true;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                FireButtonDown = false;
            }

            _mainMovement.x = horizontalMovement;
            _mainMovement.z = verticalMovement;
        }
    }
}