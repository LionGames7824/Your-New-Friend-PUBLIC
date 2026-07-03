using UnityEngine;

namespace YourNewFriend
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private Dialogue _dialogue;

        [Tooltip("If true, the dialogue can only be triggered once")]
        [SerializeField] private bool _triggerOnce = false;
        [SerializeField] private bool _save = false;

        private bool _hasTriggered = false;

        private void Awake()
        {
            if (!PlayerPrefs.HasKey("DG_SAVE")) PlayerPrefs.SetInt("DG_SAVE", 0);
        }

        public void TriggerDialogue()
        {
            if (_triggerOnce && _hasTriggered) return;
            if (PlayerPrefs.GetInt("DG_SAVE") == 1) return;

            DialogueManager.Instance.StartDialogue(_dialogue);
            _hasTriggered = true;
            if (_save) PlayerPrefs.SetInt("DG_SAVE", 1); 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                TriggerDialogue();
        }
    }
}