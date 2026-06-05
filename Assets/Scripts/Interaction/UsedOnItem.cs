using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class UsedOnItem : MonoBehaviour, IInteractable
    {
        [Header("Interaction Settings")]
        [SerializeField] private List<string> _validItemsId;
        [SerializeField] private string _interactionPrompt = "Use";
        [SerializeField] private bool _emptyHanded = false;

        [Tooltip("How many times can be used")]
        [SerializeField] private long _totalUses = 1;
        [SerializeField] private bool _autoInteract = false;

        [Header("Events")]
        [SerializeField] private UnityEvent _onCorrectItemUsed;

        private long UseCount = 0;

        public string InteractionPrompt => _interactionPrompt;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Trigger ativado por: {other.gameObject.name}");

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