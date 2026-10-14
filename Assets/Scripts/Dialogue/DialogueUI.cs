using TMPro;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace YourNewFriend
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("References TMP")]
        [SerializeField] private TextMeshProUGUI _bodyText;

        [Header("Panel")]
        [SerializeField] public GameObject _dialoguePanel;        
        [SerializeField] private Button _nextButton; 

        [Header("Timer")]
        [Range(1f, 10f)]
        [SerializeField] private float _dialogueDisplayTime = 3f;

        private Coroutine _autoAdvanceCoroutine;

        private void Start()
        {
            DialogueManager.Instance.OnDialogueStarted += HandleDialogueStarted;
            DialogueManager.Instance.OnEntryShown += HandleEntryShown;
            DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnded;
            GameStateManager.Instance.IsOpenInventory += HandleInventoryToggle; 

            gameObject.SetActive(false);

            var session = DialogueManager.Instance.ActiveSession;
            if (session != null && session.CurrentEntry != null)
            {
                HandleDialogueStarted(session.Dialogue.Title);
                HandleEntryShown(session.CurrentEntry);
            }
        }

        private void OnDestroy()
        {
            if (DialogueManager.Instance == null) return;
            DialogueManager.Instance.OnDialogueStarted -= HandleDialogueStarted;
            DialogueManager.Instance.OnEntryShown -= HandleEntryShown;
            DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnded;
            GameStateManager.Instance.IsOpenInventory -= HandleInventoryToggle; 
        }

        /// <summary>
        /// Connect this method to the "Next" button's onClick event in the Inspector.
        /// Cancels the timer and advances immediately.
        /// </summary>
        public void OnNextButtonPressed()
        {
            StopAutoAdvance();
            DialogueManager.Instance.Next();
        }

        // --- Handlers ---
        private void HandleDialogueStarted(string title)
        {
            gameObject.SetActive(true);
            _dialoguePanel.SetActive(true);
        }

        private void HandleEntryShown(DialogueEntry entry)
        {
            string body = entry.BodyText;

            if (!string.IsNullOrWhiteSpace(entry.SpeakerName))
            {
                body = $"{entry.SpeakerName}: {entry.BodyText}";
            }

            if (_bodyText) _bodyText.text = body;

            // Restart the timer
            StopAutoAdvance();
                        
            _autoAdvanceCoroutine = StartCoroutine(AutoAdvanceRoutine(entry.dialogueDisplayTime));
        }

        private void HandleDialogueEnded()
        {
            StopAutoAdvance();
            _dialoguePanel.SetActive(false);
        }

        // --- Auto advance ---
        private IEnumerator AutoAdvanceRoutine(float displayTime)
        {            
            float timeToWait = displayTime > 0 ? displayTime : _dialogueDisplayTime;

            yield return new WaitForSeconds(timeToWait);

            if (DialogueManager.Instance.IsActive)
                DialogueManager.Instance.Next();
        }

        private void StopAutoAdvance()
        {
            if (_autoAdvanceCoroutine != null)
            {
                StopCoroutine(_autoAdvanceCoroutine);
                _autoAdvanceCoroutine = null;
            }
        }

        private void HandleInventoryToggle(bool isOpen)
        {
            if (_nextButton) _nextButton.interactable = !isOpen;
        }
    }
}