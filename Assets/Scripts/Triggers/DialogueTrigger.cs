using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private Dialogue _dialogue;

        [Header("Persistence")]
        [Tooltip("Automatically generated. If you need to reset it, delete the text and save the scene.")]
        [SerializeField] private string _dialogueID;

        [Tooltip("If true, the dialogue can only be triggered once")]
        [SerializeField] private bool _triggerOnce = false;
        [SerializeField] private bool _save = false;
        [SerializeField] private bool _playOnAwake = false;

        [SerializeField] public UnityEvent _onDialogueStart;
        [SerializeField] public UnityEvent _onDialogueEnd;

        private bool _hasTriggered = false;

        private bool _specificDialogueTriggered = false;
        private bool _specificDialogueCompleted = false;

        // Reference to the DialogueUI instance (assign in inspector or will be found at runtime)
        [SerializeField] private DialogueUI _dialogueUI;

        private void Start()
        {
            if (!PlayerPrefs.HasKey(_dialogueID)) PlayerPrefs.SetInt(_dialogueID, 0);

            
            if (_playOnAwake && PlayerPrefs.GetInt(_dialogueID) == 0)
            {
                TriggerDialogue();
            }
            
        }

        void Update()
        {
            if (_dialogueUI == null)
            {
                _dialogueUI = UnityEngine.Object.FindObjectOfType<DialogueUI>();
                if (_dialogueUI == null) return;
            }

            if(_dialogueUI._dialoguePanel.activeSelf && _specificDialogueTriggered)
            {
                _onDialogueStart.Invoke();
                _specificDialogueCompleted = true;
            }

            if (!_dialogueUI._dialoguePanel.activeSelf && _specificDialogueTriggered && _specificDialogueCompleted)
            {
                _onDialogueEnd.Invoke();
                _specificDialogueTriggered = false;
                _specificDialogueCompleted = false;
            }
        }    

        public void TriggerDialogue()
        {
            if (_triggerOnce && _hasTriggered) return;
            if (PlayerPrefs.GetInt(_dialogueID) == 1) return;

            DialogueManager.Instance.StartDialogue(_dialogue);
            _specificDialogueTriggered = true;
            _hasTriggered = true;
            SaveDialogueState(_save); 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                TriggerDialogue();
        }
        private void SaveDialogueState(bool save)
        {
            if (string.IsNullOrEmpty(_dialogueID)) return;
            PlayerPrefs.SetInt(_dialogueID, save ? 1 : 0);
            PlayerPrefs.Save();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_dialogueID))
            {
                _dialogueID = System.Guid.NewGuid().ToString();
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
                Debug.Log($"ID generated for {gameObject.name}: {_dialogueID}");
            }
        }
#endif
    }
}