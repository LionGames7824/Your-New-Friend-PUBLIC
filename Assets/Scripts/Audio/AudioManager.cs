
using System;
using System.Collections;
using UnityEngine;

namespace YourNewFriend
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Libraries")]
        [SerializeField] private AudioLibrary[] _musicSounds;
        [SerializeField] private AudioLibrary[] _sfxSounds;
        [SerializeField] private AudioLibrary[] _voSounds;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _voSource;

        [Header("Music Settings")]
        [SerializeField] private float _fadeDuration = 0.5f;

        public const string MusicVolumeKey = "MusicVolume";
        public const string SFXVolumeKey = "SFXVolume";

        private float _musicVolume = 1f;
        private float _sfxVolume = 1f;

        public float CurrentMusicVolume => _musicVolume;
        public float CurrentSFXVolume => _sfxVolume;
        public bool IsVoicePlaying => _voSource != null && _voSource.isPlaying;

        private string _currentBackgroundMusic;
        private string _bufferedName;
        private bool _bufferedThemeState;

        protected override void Awake()
        {
            base.Awake();

            // Do not initialize a duplicate instance.
            if (AudioManager.Instance != this)
                return;

            _musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
            _sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

            _musicSource.volume = _musicVolume;
            _sfxSource.volume = _sfxVolume;

            //PlayMusic("escapeGuide", true);
        }

        public void Buffer(string name, bool isNewTheme)
        {
            _bufferedName = name;
            _bufferedThemeState = isNewTheme;

            Debug.Log(_bufferedName);
            Debug.Log(GetBackgroundMusic());
        }

        public void ResetBuffer()
        {
            _bufferedName = null;
            _bufferedThemeState = false;
        }

        public void PlayBuffer()
        {
            if (_bufferedName != null)
            {
                PlayMusic(_bufferedName, _bufferedThemeState);
            }
            else
            {
                Buffer("SpaceSound", true);
                PlayBuffer();
            }

            Debug.Log(_bufferedName);
            Debug.Log(GetBackgroundMusic());
        }

        public void PlayMusic(string name, bool fade)
        {
            AudioLibrary sound =
                Array.Find(_musicSounds, x => x.AudioName == name);

            if (sound == null)
            {
                Debug.LogWarning($"Music '{name}' not found!");
                return;
            }

            if (fade)
                StartCoroutine(FadeToNewMusic(sound));
            else
                PlayImmediately(sound);
        }

        public void PlayMusicWithoutSaving(string name, bool fade)
        {
            AudioLibrary sound =
                Array.Find(_musicSounds, x => x.AudioName == name);

            if (sound == null)
            {
                Debug.LogWarning($"Music '{name}' not found!");
                return;
            }

            if (fade)
                StartCoroutine(FadeToNewMusic(sound));
            else
                PlayImmediatelyWithoutSaving(sound.AudioName);
        }

        private void PlayImmediately(AudioLibrary sound)
        {
            _currentBackgroundMusic = sound.AudioName;
            _musicSource.clip = sound.AudioClip;

            if (!_musicSource.isPlaying)
                _musicSource.Play();
        }

        public void PlayImmediatelyWithoutSaving(string name)
        {
            AudioLibrary sound =
                Array.Find(_musicSounds, x => x.AudioName == name);

            if (sound == null)
            {
                Debug.LogWarning($"Music '{name}' not found!");
                return;
            }

            _musicSource.clip = sound.AudioClip;

            if (!_musicSource.isPlaying)
                _musicSource.Play();
        }

        public void PlayOnceImmediatelyWithoutSaving(string name)
        {
            AudioLibrary sound =
                Array.Find(_musicSounds, x => x.AudioName == name);

            if (sound == null)
            {
                Debug.LogWarning($"Music '{name}' not found!");
                return;
            }

            _musicSource.PlayOneShot(sound.AudioClip);
        }

        public void ResetBackgroundMusic()
        {
            _currentBackgroundMusic = null;
        }

        public void PlayVoicedDialogue(string name)
        {
            AudioLibrary sound =
                Array.Find(_voSounds, x => x.AudioName == name);

            if (sound == null)
            {
                Debug.LogWarning($"Voiced dialogue '{name}' not found!");
                return;
            }

            _voSource.PlayOneShot(sound.AudioClip);
        }

        private IEnumerator FadeToNewMusic(AudioLibrary sound)
        {
            float duration = Mathf.Max(0.01f, _fadeDuration);
            float startVolume = _musicSource.volume;

            // Fade out.
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                _musicSource.volume = Mathf.Lerp(
                    startVolume,
                    0f,
                    t / duration
                );

                yield return null;
            }

            _musicSource.volume = 0f;
            _musicSource.Stop();

            // Play the new track.
            _currentBackgroundMusic = sound.AudioName;
            _musicSource.clip = sound.AudioClip;
            _musicSource.Play();

            // Fade in to the player's selected volume.
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                _musicSource.volume = Mathf.Lerp(
                    0f,
                    _musicVolume,
                    t / duration
                );

                yield return null;
            }

            _musicSource.volume = _musicVolume;
        }

        public void PlaySFX(string name)
        {
            AudioLibrary sound =
                Array.Find(_sfxSounds, x => x.AudioName == name);

            if (sound == null)
            {
                Debug.LogWarning($"SFX '{name}' not found!");
                return;
            }

            _sfxSource.clip = sound.AudioClip;
            _sfxSource.Play();
        }

        public void ToggleMusic()
        {
            _musicSource.mute = !_musicSource.mute;
        }

        public void ToggleSFX()
        {
            _sfxSource.mute = !_sfxSource.mute;
        }

        public void MusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            _musicSource.volume = _musicVolume;

            PlayerPrefs.SetFloat(MusicVolumeKey, _musicVolume);
            PlayerPrefs.Save();
        }

        public void SFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            _sfxSource.volume = _sfxVolume;

            PlayerPrefs.SetFloat(SFXVolumeKey, _sfxVolume);
            PlayerPrefs.Save();
        }

        public string GetBackgroundMusic()
        {
            return _currentBackgroundMusic;
        }
    }
}
