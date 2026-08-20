using System;
using UnityEngine;
using System.Collections;

namespace YourNewFriend
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioLibrary[] _musicSounds;
        [SerializeField] private AudioLibrary[] _sfxSounds;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private float _fadeDuration = 1.5f; 
        private string _currentBackgroundMusic;
        private string _bufferedName;
        private bool _bufferedThemeState;

        private void Awake()
        {
            PlayMusic("escapeGuide", true);
        }

        public void Buffer (string name, bool isNewTheme)
        {
            _bufferedName = name;
            _bufferedThemeState = isNewTheme;
        }
        public void PlayBuffer()
        {
            PlayMusic(_bufferedName, _bufferedThemeState);
        }
        public void PlayMusic(string name, bool fade)
        {
            AudioLibrary sound = Array.Find(_musicSounds, x => x.AudioName == name);

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

        private void PlayImmediately(AudioLibrary sound)
        {
            _currentBackgroundMusic = sound.AudioName;
            _musicSource.clip = sound.AudioClip;
            if (!_musicSource.isPlaying) _musicSource.Play();
        }

        private IEnumerator FadeToNewMusic(AudioLibrary sound)
        {
            float startVolume = _musicSource.volume;

            // Fade out
            for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
            {
                _musicSource.volume = Mathf.Lerp(startVolume, 0, t / _fadeDuration);
                yield return null;
            }

            _musicSource.volume = 0;
            _musicSource.Stop();

            // Play new track
            _currentBackgroundMusic = sound.AudioName;
            _musicSource.clip = sound.AudioClip;
            _musicSource.Play();

            // Fade in
            for (float t = 0; t < _fadeDuration; t += Time.deltaTime)
            {
                _musicSource.volume = Mathf.Lerp(0, startVolume, t / _fadeDuration);
                yield return null;
            }

            _musicSource.volume = startVolume;
        }

        public void PlaySFX(string name)
        {
            AudioLibrary sound = Array.Find(_sfxSounds, x => x.AudioName == name);

            if (sound == null)
            {
                Debug.LogWarning($"SFX '{name}' not found!");
                return;
            }

            _sfxSource.clip = sound.AudioClip;
            _sfxSource.Play();
        }

        public void ToggleMusic() => _musicSource.mute = !_musicSource.mute;
        public void ToggleSFX() => _sfxSource.mute = !_sfxSource.mute;
        public void MusicVolume(float volume) => _musicSource.volume = volume;
        public void SFXVolume(float volume) => _sfxSource.volume = volume;
        public string GetBackgroundMusic() => _currentBackgroundMusic;
    }
}
