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
        public Animator Animator;

        public void StartDialogue(Dialogue dialogue)
        {
            IsDialogueActive = true;
            Animator.Play("show");
            _lines.Clear();

            foreach(DialogueLine dialogueLine in dialogue.DialogueLines)
            {
                _lines.Enqueue(dialogueLine);
            }

            DisplayNextDialogueLine();
        }

        public void DisplayNextDialogueLine()
        {
            if(_lines.Count > 0)
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
            _dialogueArea.text = "";
            foreach(char letter in dialogueLine.Line.ToCharArray())
            {
                _dialogueArea.text += letter;
                yield return new WaitForSeconds(TypingSpeed);
            }
        }

        private void EndDialogue()
        {
            IsDialogueActive = false;
            Animator.Play("hide");
        }

    }
}