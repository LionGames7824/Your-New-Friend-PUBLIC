
using UnityEngine;
using UnityEngine.UI;

namespace YourNewFriend
{
    public class MenuSettingsUI : MonoBehaviour
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private void Awake()
        {
            _musicSlider.onValueChanged.AddListener(MusicVolume);
            _sfxSlider.onValueChanged.AddListener(SFXVolume);
        }

        private void OnDestroy()
        {
            _musicSlider.onValueChanged.RemoveListener(MusicVolume);
            _sfxSlider.onValueChanged.RemoveListener(SFXVolume);
        }

        private void OnEnable()
        {
            if (AudioManager.Instance == null)
                return;

            _musicSlider.SetValueWithoutNotify(
                AudioManager.Instance.CurrentMusicVolume);

            _sfxSlider.SetValueWithoutNotify(
                AudioManager.Instance.CurrentSFXVolume);
        }

        public void MusicVolume()
        {
            MusicVolume(_musicSlider.value);
        }

        public void SFXVolume()
        {
            SFXVolume(_sfxSlider.value);
        }

        private void MusicVolume(float value)
        {
            AudioManager.Instance.MusicVolume(value);
        }

        private void SFXVolume(float value)
        {
            AudioManager.Instance.SFXVolume(value);
        }

        public void CloseSettings()
        {
            gameObject.SetActive(false);
        }
    }
}
