using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YourNewFriend
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [Header("References for the UI Components")]
        [SerializeField] private TextMeshProUGUI _dialogueArea;

        private Queue<DialogueLine> _lines;

        public bool IsDialogueActive = false;
        public float TypingSpeed = 0.2f;
        
        [Range(1f, 10f)]
        public float DialogueDisplayTime = 3f;

        private void Awake()
        {
            _lines = new Queue<DialogueLine>();
        }

        public void StartDialogue(Dialogue dialogue)
        {
            Debug.Log("Start dialogo");
            IsDialogueActive = true;
            //Animator.Play("show");
            _lines.Clear();

            foreach(DialogueLine dialogueLine in dialogue.DialogueLines)
            {
                _lines.Enqueue(dialogueLine);
            }

            DisplayNextDialogueLine();
        }

        public void DisplayNextDialogueLine()
        {
            if (_lines.Count == 0)
            {
                EndDialogue();
                return;
            }

            DialogueLine currentLine = _lines.Dequeue();

            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentLine));
        }

        IEnumerator TypeSentence(DialogueLine dialogueLine)
        {
            _dialogueArea.text = dialogueLine.Line;
            yield return new WaitForSeconds(DialogueDisplayTime);
            DisplayNextDialogueLine();
        }

        private void EndDialogue()
        {
            IsDialogueActive = false;
            _dialogueArea.text = "";
        }

    }
}