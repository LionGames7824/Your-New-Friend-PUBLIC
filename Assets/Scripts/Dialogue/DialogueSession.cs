using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    public class DialogueSession
    {
        public IDialogue Dialogue { get; private set; }
        public int CurrentIndex { get; private set; }
        public bool IsFinished => CurrentIndex >= Dialogue.Entries.Length;
        public DialogueEntry CurrentEntry => IsFinished ? null : Dialogue.Entries[CurrentIndex];

        public DialogueSession(IDialogue dialogue)
        {
            Dialogue = dialogue;
            CurrentIndex = 0;
        }

        /// <summary>Goes to next entry. Return false if it is the last</summary>
        public bool MoveNext()
        {
            Debug.Log($"[Session] MoveNext. Index antes: {CurrentIndex}, Length: {Dialogue.Entries.Length}");
            if (IsFinished) return false;
            CurrentIndex++;
            Debug.Log($"[Session] Index depois: {CurrentIndex}, IsFinished: {IsFinished}");
            return !IsFinished;
        }
    }
}