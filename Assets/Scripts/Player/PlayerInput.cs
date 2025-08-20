using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static YourNewFriend.Input;

namespace YourNewFriend
{
    public class PlayerInput : MonoBehaviour, IPlayerActions
    {
        public Action OnPlayerDropItem;
        public Action<float> OnPlayerSprint;
        public Action<float> OnPlayerCrouch;
        public Action<float> OnPlayerJump;
        public Action<float> OnPlayerInteract;
        public Action<Vector2> OnPlayerMove;
        public Action<Vector2> OnPlayerCamera;
        private Input _input;
             
        private void Awake()
        {
            _input = new();
            _input.Player.SetCallbacks(this);
            _input.Player.Enable();
            ShowMouse(false);
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

        void IPlayerActions.OnCrouch(InputAction.CallbackContext context)
        {
            OnPlayerCrouch?.Invoke(context.ReadValue<float>());
        }

        void IPlayerActions.OnSprint(InputAction.CallbackContext context)
        {
            OnPlayerSprint?.Invoke(context.ReadValue<float>());
        }
        void IPlayerActions.OnJump(InputAction.CallbackContext context)
        {
            OnPlayerJump?.Invoke(context.ReadValue<float>());
        }

        void IPlayerActions.OnMoveCamera(InputAction.CallbackContext context)
        {
            OnPlayerCamera?.Invoke(context.ReadValue<Vector2>());
        }

        void IPlayerActions.OnInteractable(InputAction.CallbackContext context)
        {
            OnPlayerInteract?.Invoke(context.ReadValue<float>());
        }

        void IPlayerActions.OnInventory(InputAction.CallbackContext context)
        {
            // Toggle the inventory state
            bool isCurrentlyOpen = GameStateManager.Instance.HasInventoryOpen;
            bool isCurrentlyHoldingItem = !string.IsNullOrEmpty(InventoryManager.Instance._itemEquipped);

            if (isCurrentlyOpen)
            {
                // Close inventory
                ResumeGame();
                GameStateManager.Instance.OpenInventory(false);
            }
            else if (!isCurrentlyHoldingItem)  
            {
                // Open inventory
                Time.timeScale = 0.0f;
                ShowMouse(true);
                GameStateManager.Instance.OpenInventory(true);
            }
        }

        public void OnDropItem(InputAction.CallbackContext context)
        {
            OnPlayerDropItem?.Invoke();
        }

        public void ShowMouse(bool show)
        {
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = show;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1.0f;
            ShowMouse(false);
        }

    }
}