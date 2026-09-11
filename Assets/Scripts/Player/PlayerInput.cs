using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static YourNewFriend.Input;

namespace YourNewFriend
{
    public class PlayerInput : MonoBehaviour, IPlayerActions
    {
        public Action OnPlayerDropItem;
        public Action OnPlayerUnequipItem;
        public Action OnPlayerShoot;
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
            if (!context.performed) return;
            OnPlayerInteract?.Invoke(context.ReadValue<float>());
        }

        void IPlayerActions.OnInventory(InputAction.CallbackContext context)
        {
            bool isCurrentlyOpen = GameStateManager.Instance.HasInventoryOpen;
            bool isCurrentlyHoldingItem = !string.IsNullOrEmpty(InventoryManager.Instance._itemEquipped);
            bool isTheSettingsOpen = GameStateManager.Instance.HasSettingsOpen;

            // If the player is holding an item and the inventory is closed, don't open it
            if (!isCurrentlyOpen && isCurrentlyHoldingItem || isTheSettingsOpen)
            {
                Debug.Log($"{GetType().Name}:Cannot open inventory");
                return;
            }

            if (isCurrentlyOpen)
            {
                // Close inventory
                ResumeGame();
                GameStateManager.Instance.OpenInventory(false);
            }
            else
            {
                // Open inventory
                ShowMouse(true);
                GameStateManager.Instance.OpenInventory(true);
            }
        }

        void IPlayerActions.OnSettings(InputAction.CallbackContext context)
        {
            bool isCurrentlyOpen = GameStateManager.Instance.HasSettingsOpen;         

            if (isCurrentlyOpen)
            {
                // Close Settings
                ResumeGame();
                GameStateManager.Instance.OpenSettings(false);
            }
            else
            {
                // Open Settings
                Time.timeScale = 0.0f;
                ShowMouse(true);
                GameStateManager.Instance.OpenSettings(true);
            }
        }

        void IPlayerActions.OnDropItem(InputAction.CallbackContext context)
        {
            OnPlayerDropItem?.Invoke();
        }

        void IPlayerActions.OnSkipDialogue(InputAction.CallbackContext context)
        {
            if (context.performed && DialogueManager.Instance.IsActive)
                DialogueManager.Instance.Next();
        }

        void IPlayerActions.OnUnequipItem(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            bool isHoldingItem = !string.IsNullOrEmpty(InventoryManager.Instance._itemEquipped);
            if (!isHoldingItem) return;

            OnPlayerUnequipItem?.Invoke();
        }

        void IPlayerActions.OnShoot(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            OnPlayerShoot?.Invoke();
        }

        public void ShowMouse(bool show)
        {
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = show;

            // While the cursor is locked, the Input System keeps its tracked mouse
            // position frozen at wherever it was when it got locked - it only resyncs
            // once real mouse movement is detected. That means the first UI clicks
            // right after unlocking (e.g. the Game Over screen's Resume button) can
            // land on a stale position and miss, until the player happens to move the
            // mouse enough for it to catch up. Warping to the center forces an
            // immediate, correct position update so the very next click is accurate.
            if (show && Mouse.current != null)
                Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2f, Screen.height / 2f));
        }

        public void ResumeGame()
        {
            Time.timeScale = 1.0f;
            ShowMouse(false);
        }

    }
}