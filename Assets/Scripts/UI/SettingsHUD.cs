using UnityEngine;
using UnityEngine.UI;

namespace YourNewFriend
{
    public class SettingsHUD : MonoBehaviour
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private void Start()
        {
            gameObject.SetActive(false);

            GameStateManager.Instance.IsOpenSettings += OpenSettings;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.IsOpenSettings -= OpenSettings;
        }

        public void ToggleMusic()
        {
            AudioManager.Instance.ToggleMusic();
        }

        public void ToggleSFX()
        {
            AudioManager.Instance.ToggleSFX();
        }

        public void MusicVolume()
        {
            AudioManager.Instance.MusicVolume(_musicSlider.value);
        }

        public void SFXVolume()
        {
            AudioManager.Instance.SFXVolume(_sfxSlider.value);
        }

        private void OpenSettings(bool isOpen)
        {
            gameObject.SetActive(isOpen);
        }
    }
}