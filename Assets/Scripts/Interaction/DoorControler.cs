using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class DoorController : MonoBehaviour
    {
        [Header("Persistence")]
        [Tooltip("Automatically generated. If you need to reset it, delete the text and save the scene.")]
        [SerializeField] private string _doorID;

        [Header("Settings")]
        [Tooltip("Check this to make the door start open by default (when there is no save data).")]
        [SerializeField] private bool _startOpened = false;

        [Header("Events")]
        [SerializeField] private UnityEvent _onDoorOpened;

        private Animator _anim;
        private string _doorState = "Door_State";

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        private void Start()
        {
            LoadDoorState();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {            
            if (string.IsNullOrEmpty(_doorID))
            {         
                _doorID = System.Guid.NewGuid().ToString();
                             
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
                Debug.Log($"Unique ID automatically generated for {gameObject.name}: {_doorID}");
            }
        }
#endif

        public void ToggleDoor()
        {
            bool isOpening = !_anim.GetBool(_doorState);
            _anim.SetBool(_doorState, isOpening);

            SaveDoorState(isOpening);

            if (isOpening)
            {
                _onDoorOpened.Invoke();
                Debug.Log($"Door {_doorID} open and state saved");
            }
        }

        private void SaveDoorState(bool open)
        {
            if (string.IsNullOrEmpty(_doorID)) return;
            PlayerPrefs.SetInt(_doorID, open ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadDoorState()
        {
            if (string.IsNullOrEmpty(_doorID)) return;

            if (PlayerPrefs.HasKey(_doorID))
            {
                bool savedAsOpen = PlayerPrefs.GetInt(_doorID, 0) == 1;
                if (savedAsOpen)
                {
                    _anim.SetBool(_doorState, true);
                    _onDoorOpened.Invoke();
                }
            }
            else
            {
                if (_startOpened)
                {
                    _anim.SetBool(_doorState, true);
                    _onDoorOpened.Invoke();
                }
            }
        }
    }
}