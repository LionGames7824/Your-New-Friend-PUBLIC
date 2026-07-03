using UnityEngine;

namespace YourNewFriend
{
    public class MusicTriggerZone : MonoBehaviour
    {
        [SerializeField] private string _musicName;
        private string _previousTheme;
        private const string _playerTag = "Player";
        private bool _hasStoredPreviousTheme = false;
        private bool _sameTheme = false;

        private void OnTriggerEnter(Collider collision)
        {
            if (!collision.CompareTag(_playerTag)) return;

            // Only store the previous theme the first time we enter
            if (!_hasStoredPreviousTheme && AudioManager.Instance.GetBackgroundMusic() != null)
            {
                _previousTheme = AudioManager.Instance.GetBackgroundMusic();
                _hasStoredPreviousTheme = true;
            }
            if (_previousTheme != null && _previousTheme.Equals(_musicName))
            {
                _sameTheme = true;
            }
            else
            {
                _sameTheme = false;
            }
            AudioManager.Instance.PlayMusic(_musicName, !_sameTheme);
        }

        private void OnTriggerExit(Collider collision)
        {
            if (!collision.CompareTag(_playerTag)) return;

            // Go back to the previous track if it was stored
            if (_hasStoredPreviousTheme && !string.IsNullOrEmpty(_previousTheme))
            {
                AudioManager.Instance.PlayMusic(_previousTheme, false);
                _hasStoredPreviousTheme = false; // Reset for next entry
            }
        }
    }
}
