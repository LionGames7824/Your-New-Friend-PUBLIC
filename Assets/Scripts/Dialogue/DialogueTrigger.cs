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
        private const string _playerTag = "Player";

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag(_playerTag))
            {
                TriggerDialogue();
            }
        }

        public void TriggerDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}