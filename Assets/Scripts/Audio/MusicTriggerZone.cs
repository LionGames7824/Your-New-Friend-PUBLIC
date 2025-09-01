using UnityEngine;

namespace YourNewFriend
{
    public class MusicTriggerZone : MonoBehaviour
    {
        [SerializeField] private string _musicName;
        private string _previousTheme;
        private const string _playerTag = "Player";
        private bool _hasStoredPreviousTheme = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_playerTag)) return;

            // Only store the previous theme the first time we enter
            if (!_hasStoredPreviousTheme)
            {
                _previousTheme = AudioManager.Instance.GetBackgroundMusic();
                _hasStoredPreviousTheme = true;
            }

            AudioManager.Instance.PlayMusic(_musicName);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(_playerTag)) return;

            // Go back to the previous track if it was stored
            if (_hasStoredPreviousTheme && !string.IsNullOrEmpty(_previousTheme))
            {
                AudioManager.Instance.PlayMusic(_previousTheme);
                _hasStoredPreviousTheme = false; // Reset for next entry
            }
        }
    }
}
