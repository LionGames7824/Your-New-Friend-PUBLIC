using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{

    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(3,10)]
        public string Line;
    }

    [System.Serializable]
    public class Dialogue
    {
        public List<DialogueLine> DialogueLines = new List<DialogueLine>();
    }

    public class DialogueTrigger : MonoBehaviour
    {
        public Dialogue dialogue;

        [SerializeField] private bool _triggerOnce = false; 
        [SerializeField] private string _disableCondition = "";

        private const string _playerTag = "Player";
        private bool _alreadyTriggered = false;

        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log($"{GetType().Name}:Colison");
            if (!collision.CompareTag(_playerTag)) return;
                        
            if (_triggerOnce && _alreadyTriggered) return;

            if (!string.IsNullOrEmpty(_disableCondition) &&
                InventoryManager.Instance.CheckItem(_disableCondition)) return;

            TriggerDialogue();
        }

        public void TriggerDialogue()
        {
            _alreadyTriggered = true;
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}