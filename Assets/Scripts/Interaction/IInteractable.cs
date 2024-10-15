using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    public interface IInteractable
    {
        public string InteractionPrompt { get; }
        public bool Interact(Player_Interactor interactor);
    }
}