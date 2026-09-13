using UnityEngine;

namespace YourNewFriend
{
    public class DialogueVOTriggerZone : MonoBehaviour
    {
        [Header("Persistence")]
        [Tooltip("Automatically generated. If you need to reset it, delete the text and save the scene.")]
        [SerializeField] private string _voID;
        [SerializeField] private string _dialogueName;
        private const string _playerTag = "Player";
        [SerializeField] private bool _triggerOnce = false;
        [SerializeField] private bool _save = false;
        private bool _hasTriggered = false;
        private void Awake()
        {
            if (!PlayerPrefs.HasKey(_voID)) PlayerPrefs.SetInt(_voID, 0);
        }
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag(_playerTag))
            {
                TriggerVoiceOver();
            }
        }

        public void TriggerVoiceOver()
        {
            if (_triggerOnce && _hasTriggered) return;
            if (PlayerPrefs.GetInt(_voID) == 1) return;
            AudioManager.Instance.PlayVoicedDialogue(_dialogueName);
            _hasTriggered = true;
            SaveVOState(_save);
        }

        private void SaveVOState(bool save)
        {
            if (string.IsNullOrEmpty(_voID)) return;
            PlayerPrefs.SetInt(_voID, save ? 1 : 0);
            PlayerPrefs.Save();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_voID))
            {
                _voID = System.Guid.NewGuid().ToString();
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
                Debug.Log($"ID generated for {gameObject.name}: {_voID}");
            }
        }
#endif
    }
}
