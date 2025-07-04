using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class UsedOnItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private List<string> _validItemsId;
        [SerializeField] private UnityEvent _onCorrectItemUsed;
        [SerializeField] private string _interactionPrompt = "Use";
        [SerializeField] private Animator anim;
        [SerializeField] private string _animTriggerName = "";

        public string InteractionPrompt => _interactionPrompt;

        public bool Interact (PlayerController interaction)
        {
            if (_validItemsId.Contains(InteractionManager.Instance.CurrentItemId))
            {
                Debug.Log("Correct Item Used");
                _onCorrectItemUsed.Invoke();

                if (anim != null)
                {
                    anim.SetTrigger(_animTriggerName);
                }
                return true;
            }
            return false;
        }
    }
}