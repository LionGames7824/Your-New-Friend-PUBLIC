using TMPro;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

namespace YourNewFriend
{
    public class DialogueUI : MonoBehaviour
    {
        [Header("References TMP")]
        [SerializeField] private TextMeshProUGUI _bodyText;

        [Header("Panel")]
        [SerializeField] private GameObject _dialoguePanel;

        [Header("Timer")]
        [Range(1f, 10f)]
        [SerializeField] private float _dialogueDisplayTime = 3f;

        private Coroutine _autoAdvanceCoroutine;

        private void Start()
        {
            DialogueManager.Instance.OnDialogueStarted += HandleDialogueStarted;
            DialogueManager.Instance.OnEntryShown += HandleEntryShown;
            DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnded;
        }

        private void OnDestroy()
        {
            if (DialogueManager.Instance == null) return;
            DialogueManager.Instance.OnDialogueStarted -= HandleDialogueStarted;
            DialogueManager.Instance.OnEntryShown -= HandleEntryShown;
            DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnded;
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
            _autoAdvanceCoroutine = StartCoroutine(AutoAdvanceRoutine());
        }

        private void HandleDialogueEnded()
        {
            StopAutoAdvance();
            _dialoguePanel.SetActive(false);
        }

        // --- Auto advance ---
        private IEnumerator AutoAdvanceRoutine()
        {
            yield return new WaitForSeconds(_dialogueDisplayTime);
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
    }
}