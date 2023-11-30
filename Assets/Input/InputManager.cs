using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        public Vector2 move { get; private set; }
        public bool isRunning { get; private set; }

        private PlayerControls playerControls;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;
            playerControls = new PlayerControls();

            playerControls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            playerControls.Player.Move.canceled += ctx => move = Vector2.zero;

            playerControls.Player.Run.performed += ctx => isRunning = true;
            playerControls.Player.Run.canceled += ctx => isRunning = false;
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}
