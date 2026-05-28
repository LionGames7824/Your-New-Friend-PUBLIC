using System;
using UnityEngine;

namespace YourNewFriend
{
    [Serializable]
    public class DialogueEntry
    {
        [Tooltip("Name of the Speaker")]
        public string SpeakerName;

        [Tooltip("Text of the entry")]
        [TextArea(2, 6)]
        public string BodyText;

        // TODO: We can put an action/event here latter
        // Like the cinemachine stuff
    }
}