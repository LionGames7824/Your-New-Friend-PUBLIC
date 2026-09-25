using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace YourNewFriend
{
    public class TVTrigger : MonoBehaviour
    {

        [Header("Persistence")]
        [Tooltip("Automatically generated. If you need to reset it, delete the text and save the scene.")]
        [SerializeField] private string _tvID;
        [SerializeField] private VideoPlayer _tv;
        [SerializeField] private bool _triggerOnce = false;
        [SerializeField] private bool _save = false;
        private bool _hasTriggered = false;
        private const string _playerTag = "Player";
        public AudioSource _voSource;

        public VideoClip _clip;
        public VideoClip _defaultClip;
        private bool flag = false;
        private void Awake()
        {
            if (!PlayerPrefs.HasKey(_tvID)) PlayerPrefs.SetInt(_tvID, 0);
        }
        void Update()
        {
            if (!_voSource.isPlaying && !flag)
            {
                _tv.clip = _defaultClip;
                _tv.Play();
                flag = true;
            }
        }
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag(_playerTag)) 
                TriggerTV();
                flag = false;
        }

        public void TriggerTV()
        {
            if (_triggerOnce && _hasTriggered) return;
            if (PlayerPrefs.GetInt(_tvID) == 1) return;
            _tv.clip = _clip;
            _tv.Play();
            Debug.Log("TV Triggered");
            _hasTriggered = true;
            SaveTVState(_save);
        }

        private void SaveTVState(bool save)
        {
            if (string.IsNullOrEmpty(_tvID)) return;
            PlayerPrefs.SetInt(_tvID, save ? 1 : 0);
            PlayerPrefs.Save();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_tvID))
            {
                _tvID = System.Guid.NewGuid().ToString();
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
                Debug.Log($"ID generated for {gameObject.name}: {_tvID}");
            }
        }
#endif
    }
}
