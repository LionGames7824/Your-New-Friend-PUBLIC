using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class UsedOnItem : MonoBehaviour, IInteractable
    {
        [Header("Interaction Settings")]
        [SerializeField] private List<string> _validItemsId;
        [SerializeField] private string _interactionPrompt = "";
        [SerializeField] private bool _emptyHanded = false;

        [Tooltip("How many times can be used")]
        [SerializeField] private long _totalUses = 1;
        [SerializeField] private bool _autoInteract = false;

        [Tooltip("If enabled, the use count resets when the player respawns after Game Over (e.g. a lever you should be able to retry).")]
        [SerializeField] private bool _resetOnGameOver = false;
        [SerializeField] private UnityEvent _onGameOver;

        [Header("Events")]
        [SerializeField] private UnityEvent _onCorrectItemUsed;

        private long UseCount = 0;

        public string InteractionPrompt => _interactionPrompt;

        private void OnEnable()
        {
            if (_resetOnGameOver)
                GameStateManager.Instance.IsGameOver += HandleGameOver;
        }

        private void OnDisable()
        {
            if (_resetOnGameOver && GameStateManager.Instance != null)
                GameStateManager.Instance.IsGameOver -= HandleGameOver;
        }

        private void HandleGameOver(bool isOpen)
        {
            if (!isOpen) ResetUses();
            _onGameOver?.Invoke();
        }

        public void ToggleGameOver(bool gameOver)
        {
            _resetOnGameOver = gameOver;
        }

        public void ResetUses()
        {
            UseCount = 0;
        }

        public void ReduceUses()
        {
            UseCount--;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (!_autoInteract) return;

            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                Interact(player);
            }
        }

        public bool Interact(PlayerController interaction)
        {            
            bool hasUsesLeft = (_totalUses == 0) || (UseCount < _totalUses);

            if (hasUsesLeft && (_validItemsId.Contains(InventoryManager.Instance._itemEquipped) || _emptyHanded))
            {
                _onCorrectItemUsed.Invoke();

                UseCount++;
                return true;
            }
                        
            return false;
        }
    }
}