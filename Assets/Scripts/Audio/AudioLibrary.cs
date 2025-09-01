using UnityEngine;

namespace YourNewFriend
{
    [System.Serializable]
    public class AudioLibrary 
    {
        [SerializeField] public string AudioName;
        [SerializeField] public AudioClip AudioClip;
    }
}