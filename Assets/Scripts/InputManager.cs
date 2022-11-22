using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Player _player;

        private PlayerInputActions _input;

        public void Start()
        {
            SetupEvents();
            _input.Enable();
        }

        public void Update()
        {
            Movement();
        }

        private void OnDestroy()
        {
            DestroyEvents();
        }

        private void SetupEvents()
        {
            _input = new PlayerInputActions();
            _input.Player.Jump.performed += Jump;
            _input.Player.Sit.performed += Sit;
        }

        private void DestroyEvents()
        {
            _input.Player.Jump.performed -= Jump;
            _input.Player.Sit.performed -= Sit;
        }

        private void Movement()
        {
            _player.Move(_input.Player.Movement.ReadValue<Vector2>());
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            _player.Jump();
        }

        private void Sit(InputAction.CallbackContext ctx)
        {
            _player.Sit();
        }
    }
}
