using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    public class PickUpItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _itemID;
        [SerializeField] private int quantity = 1;
        [SerializeField] private string _interactionPrompt = "Pick Up";
        public string InteractionPrompt => _interactionPrompt;

        public bool Interact (PlayerController interactor)
        {
            InventoryManager.Instance.AddItem(_itemID, quantity);
            return true;
        }
    }
}