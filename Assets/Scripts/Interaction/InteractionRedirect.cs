using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    public class InteractionRedirect : MonoBehaviour, IInteractable
    {
        private IInteractable _parentInteractable;

        private void Awake()
        {
            var all = GetComponentsInParent<IInteractable>();
            foreach (var interactable in all)
            {
                if (interactable != this)
                {
                    _parentInteractable = interactable;
                    break;
                }
            }
        }
        public string InteractionPrompt => _parentInteractable?.InteractionPrompt;
        public bool Interact(PlayerController interactor)
        {
            return _parentInteractable?.Interact(interactor) ?? false;
        }
    }
}
