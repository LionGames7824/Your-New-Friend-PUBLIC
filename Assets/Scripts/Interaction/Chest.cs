using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt;
        public string InteractionPrompt => _prompt;

        public bool Interact(Player_Interactor interactor)
        {
            Debug.Log("Opening chest");
            return true;
        }
    }
}