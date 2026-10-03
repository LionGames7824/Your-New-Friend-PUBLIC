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
        public static bool mus_overlap = false;
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
            AudioManager.Instance.Buffer(_musicName, !_sameTheme);
            if (!mus_overlap)
            {
                AudioManager.Instance.PlayBuffer();
            }
        }

        public static void TurnOffOverlap()
        {
            mus_overlap = false;
        }

        public static void TurnOnOverlap()
        {
            mus_overlap = true;
        }

        private void OnTriggerExit(Collider collision)
        {
            if (!collision.CompareTag(_playerTag)) return;

            // Go back to the previous track if it was stored
            if (!mus_overlap)
            {
                if (_hasStoredPreviousTheme && !string.IsNullOrEmpty(_previousTheme))
                {
                    AudioManager.Instance.PlayMusic(_previousTheme, false);
                    _hasStoredPreviousTheme = false; // Reset for next entry
                }
                else
                {
                    AudioManager.Instance.Buffer("SpaceSound", true);
                    AudioManager.Instance.PlayBuffer();
                }
            }
        }
    }
}
