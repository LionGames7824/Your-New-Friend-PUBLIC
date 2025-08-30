using System;
using UnityEngine;

namespace YourNewFriend
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioLibrary[] _musicSounds;
        [SerializeField] private AudioLibrary[] _sfxSounds;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        private void Awake()
        {
            PlayMusic("escapeGuide");
        }

        public void PlayMusic(string name)
        {
            AudioLibrary sound = Array.Find(_musicSounds, x => x.AudioName == name);

            if (sound == null) {
                Debug.Log("Sound Not found");
            }
            else
            {
                _musicSource.clip = sound.AudioClip;
                _musicSource.Play();
            }           
        }

        public void PlaySFX(string name)
        {
            AudioLibrary sound = Array.Find(_sfxSounds, x => x.AudioName == name);

            if (sound == null)
            {
                Debug.Log("Sound Not found");
            }
            else
            {
                _sfxSource.clip = sound.AudioClip;
                _sfxSource.Play();
            }
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
            _musicSource.volume = volume;
        }

        public void SFXVolume(float volume)
        {
            _sfxSource.volume = volume;
        }
    }
}