using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YourNewFriend
{
    public class DialogueManager : Singleton<DialogueManager>
    {        
        protected override void Awake()
        {
            base.Awake();
            //DontDestroyOnLoad(gameObject);
        }

        // State 
        public DialogueSession ActiveSession { get; private set; }
        public bool IsActive => ActiveSession != null;

        // Events 
        public event Action<string> OnDialogueStarted;
        public event Action<DialogueEntry> OnEntryShown;
        public event Action OnDialogueEnded;

        public void StartDialogue(IDialogue dialogue)
        {
            if (IsActive)
            {                
                return;
            }

            ActiveSession = new DialogueSession(dialogue);
            OnDialogueStarted?.Invoke(dialogue.Title);
            ShowCurrentEntry();
        }

        public void Next()
        {
            Debug.Log($"[DM] Next called. IsActive: {IsActive}");
            if (!IsActive) return;

            bool hasNext = ActiveSession.MoveNext();
            Debug.Log($"[DM] hasNext: {hasNext}, index: {ActiveSession.CurrentIndex}");

            if (hasNext)
                ShowCurrentEntry();
            else
                EndDialogue();
        }

        public void DismissDialogue()
        {
            if (!IsActive) return;
            EndDialogue();
        }
                
        private void ShowCurrentEntry()
        {
            var entry = ActiveSession.CurrentEntry;
            OnEntryShown?.Invoke(entry);
        }

        private void EndDialogue()
        {
            Debug.Log("[DM] EndDialogue call");
            ActiveSession = null;
            OnDialogueEnded?.Invoke();
        }

    }
}