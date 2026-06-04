using UnityEngine;

namespace YourNewFriend
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private Dialogue _dialogue;

        [Tooltip("If true, the dialogue can only be triggered once")]
        [SerializeField] private bool _triggerOnce = false;

        private bool _hasTriggered = false;

        public void TriggerDialogue()
        {
            if (_triggerOnce && _hasTriggered) return;

            DialogueManager.Instance.StartDialogue(_dialogue);
            _hasTriggered = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                TriggerDialogue();
        }
    }
}