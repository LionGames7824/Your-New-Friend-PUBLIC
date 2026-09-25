using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class CutsceneTrigger : MonoBehaviour
    {
        [Header("Persistence")]
        [Tooltip("Automatically generated. If you need to reset it, delete the text and save the scene.")]
        [SerializeField] private string _cutsceneID;

        [Tooltip("If true, the dialogue can only be triggered once")]
        [SerializeField] private bool _triggerOnce = false;
        [SerializeField] private bool _save = false;


        [SerializeField] private UnityEvent _onCutsceneStart;
        private bool _hasTriggered = false;

        public PlayableDirector playableDirector;
        public TimelineAsset cutscene;

        private void Awake()
        {
            if (!PlayerPrefs.HasKey(_cutsceneID)) PlayerPrefs.SetInt(_cutsceneID, 0);
        }
        public void TriggerCutscene()
        {
            if (_triggerOnce && _hasTriggered) return;
            if (PlayerPrefs.GetInt(_cutsceneID) == 1) return;

            _onCutsceneStart.Invoke();
            playableDirector.Play(cutscene);
            _hasTriggered = true;
            SaveCutsceneState(_save);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                TriggerCutscene();
        }

        private void SaveCutsceneState(bool save)
        {
            if (string.IsNullOrEmpty(_cutsceneID)) return;
            PlayerPrefs.SetInt(_cutsceneID, save ? 1 : 0);
            PlayerPrefs.Save();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_cutsceneID))
            {
                _cutsceneID = System.Guid.NewGuid().ToString();
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
                Debug.Log($"ID generated for {gameObject.name}: {_cutsceneID}");
            }
        }
#endif
    }
}