using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        [SerializeField] private UnityEvent _onCorrectItemUsed_Open;
        [SerializeField] private UnityEvent _onCorrectItemUsed_Close;
        [SerializeField] private string _interactionPrompt = "Use";
        [SerializeField] private List<Animator> anim;
        [SerializeField] private List<string> _animTriggerName;
        [SerializeField] private List<string> _animBoolName;
        [SerializeField] private bool _emptyHanded = false;
        [SerializeField] private long _totalUses = 1;
        [SerializeField] private List<bool> _isitadoor;

        private List<bool> animplaying;
        private void Awake()
        {
            animplaying = new List<bool>(new bool[anim.Count]);
        }
        IEnumerator AnimationPlaying(Animator anim, int i)
        {
            animplaying[i] = true;
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            animplaying[i] = false;
        }

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
                    if (_isitadoor[i])
                    {
                        if (!animplaying[i])
                        {
                            anim[i].SetBool("Door_State", !anim[i].GetBool("Door_State"));
                            if (anim[i].GetBool("Door_State"))
                            {
                                _onCorrectItemUsed_Close.Invoke();
                            }
                            else
                            {
                                _onCorrectItemUsed_Open.Invoke();
                            }
                            if (!(_animTriggerName[i] == ""))
                            {
                                anim[i].SetTrigger(_animTriggerName[i]);
                                StartCoroutine(AnimationPlaying(anim[i], i));
                            }
                            if (!(_animBoolName[i] == ""))
                            {
                                anim[i].SetBool(_animBoolName[i], !anim[i].GetBool(_animBoolName[i]));
                            }
                        }
                        UseCount--;
                    }
                    else
                    {
                        if (!animplaying[i])
                        {
                            if (!(_animTriggerName[i] == ""))
                            {
                                anim[i].SetTrigger(_animTriggerName[i]);
                                StartCoroutine(AnimationPlaying(anim[i], i));
                            }
                            if (!(_animBoolName[i] == ""))
                            {
                                anim[i].SetBool(_animBoolName[i], !anim[i].GetBool(_animBoolName[i]));
                            }
                        }
                        
                    }
                    
                }
                UseCount++;
                return true;
            }
            return false;
        }
    }   
}