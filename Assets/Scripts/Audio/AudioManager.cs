using System;
using UnityEngine;

namespace YourNewFriend
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioLibrary[] _musicSounds;
        [SerializeField] private AudioLibrary[] _soundEffectSounds;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _SoundEffectSource;

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

        public void PlaySoundEffect(string name)
        {
            AudioLibrary sound = Array.Find(_soundEffectSounds, x => x.AudioName == name);

            if (sound == null)
            {
                Debug.Log("Sound Not found");
            }
            else
            {
                _SoundEffectSource.clip = sound.AudioClip;
                _SoundEffectSource.Play();
            }
        }
    }
}