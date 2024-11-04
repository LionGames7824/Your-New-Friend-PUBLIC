using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static YourNewFriend.Input;

namespace YourNewFriend
{
    public class PlayerInput : MonoBehaviour, IPlayerActions
    {
        public Action<float> OnPlayerJump;
        public Action<Vector2> OnPlayerMove;
        public Action<Vector2> OnPlayerCamera;
        private Input _input;

        private void Awake()
        {
            _input = new();
            _input.Player.SetCallbacks(this);
            _input.Player.Enable();
        }
        private void OnDestroy()
        {
            _input?.Player.SetCallbacks(null);
            _input?.Player.Disable();
        }

        void IPlayerActions.OnPlayerMove(InputAction.CallbackContext context)
        {
            OnPlayerMove?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            OnPlayerJump?.Invoke(context.ReadValue<float>());
        }

        public void OnMoveCampera(InputAction.CallbackContext context)
        {
            OnPlayerCamera?.Invoke(context.ReadValue<Vector2>());
        }
    }
}