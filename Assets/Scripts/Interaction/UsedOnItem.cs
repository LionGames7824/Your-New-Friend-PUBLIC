using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class UsedOnItem : MonoBehaviour, IInteractable
    {
        [System.Serializable]
        private class Data
        {
            public enum State
            {
                Interactable, // Player can interact with an object
                NotInteractable, // Player can't interact with an object
                Used // Player already interacted with an object
            }

            public State ObjectState;
        }


        [SerializeField] private List<string> _validItemsId;
        [SerializeField] private UnityEvent _onCorrectItemUsed;
        [SerializeField] private string _interactionPrompt = "Use";
        [SerializeField] private List<Animator> anim;
        [SerializeField] private List<string> _animTriggerName;
        [SerializeField] private bool _emptyHanded = false;
        [SerializeField] private long _totalUses = 1;

        private long UseCount = 0;
        public string InteractionPrompt => _interactionPrompt;

        public bool Interact (PlayerController interaction)
        {
            if (UseCount < _totalUses && (_validItemsId.Contains(InventoryManager.Instance._itemEquipped) || _emptyHanded))
            {
                Debug.Log("Correct Item Used");
                _onCorrectItemUsed.Invoke();

                for (int i = 0; i < anim.Count; i++)
                {
                    anim[i].SetTrigger(_animTriggerName[i]);
                }
                UseCount++;
                return true;
            }
            return false;
        }
    }
}