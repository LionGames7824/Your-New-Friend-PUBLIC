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

        [Header("Interaction Settings")]
        [SerializeField] private List<string> _validItemsId;
        [SerializeField] private string _interactionPrompt = "Use";
        [SerializeField] private bool _emptyHanded = false;
        [SerializeField] private long _totalUses = 1;
        [SerializeField] private bool _autoInteract = false;

        [Header("Animation Settings")]
        [SerializeField] private List<Animator> anim;
        [SerializeField] private List<string> _animTriggerName;
        [SerializeField] private List<string> _animBoolName;
        [SerializeField] private List<bool> _isitadoor;
        private const string DoorStateParam = "Door_State";

        [Header("Events")]
        [SerializeField] private UnityEvent _onCorrectItemUsed;
        [SerializeField] private UnityEvent _onCorrectItemUsed_Open;
        [SerializeField] private UnityEvent _onCorrectItemUsed_Close;

        private List<bool> animplaying;
        private long UseCount = 0;

        public string InteractionPrompt => _interactionPrompt;

        private void Awake()
        {
            animplaying = new List<bool>(new bool[anim.Count]);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Trigger ativado por: {other.gameObject.name}");

            if (!_autoInteract)
            {
                Debug.Log("_autoInteract está false, saindo");
                return;
            }

            PlayerController player = other.GetComponent<PlayerController>();
            Debug.Log($"PlayerController encontrado: {player != null}");

            if (player != null && !_autoInteract)
                Interact(player);
        }

        IEnumerator AnimationPlaying(Animator anim, int i)
        {
            animplaying[i] = true;
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            animplaying[i] = false;
        }

        public bool Interact(PlayerController interaction)
        {
            Debug.Log($"itemEquipped: '{InventoryManager.Instance._itemEquipped}'");
            Debug.Log($"validItems: '{string.Join(",", _validItemsId)}'");
            Debug.Log($"Contains: {_validItemsId.Contains(InventoryManager.Instance._itemEquipped)}");

            if (UseCount < _totalUses && (_validItemsId.Contains(InventoryManager.Instance._itemEquipped) || _emptyHanded))
            {
                _onCorrectItemUsed.Invoke();

                for (int i = 0; i < anim.Count; i++)
                {
                    if (_isitadoor[i])
                    {
                        if (!animplaying[i])
                        {
                            anim[i].SetBool(DoorStateParam, !anim[i].GetBool(DoorStateParam));
                            if (anim[i].GetBool(DoorStateParam))
                                _onCorrectItemUsed_Close.Invoke();
                            else
                                _onCorrectItemUsed_Open.Invoke();

                            if (!(_animTriggerName[i] == ""))
                            {
                                anim[i].SetTrigger(_animTriggerName[i]);
                                StartCoroutine(AnimationPlaying(anim[i], i));
                            }
                            if (!(_animBoolName[i] == ""))
                                anim[i].SetBool(_animBoolName[i], !anim[i].GetBool(_animBoolName[i]));
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
                                anim[i].SetBool(_animBoolName[i], !anim[i].GetBool(_animBoolName[i]));
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