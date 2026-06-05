using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class DoorController : MonoBehaviour
    {
        [Header("Persistence")]
        [Tooltip("Automatically generated. If you need to reset it, delete the text and save the scene.")]
        [SerializeField] private string _doorID;

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
            // If the door doesn't have an ID (because it was just created or duplicated)
            if (string.IsNullOrEmpty(_doorID))
            {
                // Generates a GUID (Globally Unique Identifier), e.g.: f81d4fae-7dec-11d0-a765-00a0c91e6bf6
                _doorID = System.Guid.NewGuid().ToString();

                // Notifies Unity that the scene has changed and needs to be saved
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

            bool savedAsOpen = PlayerPrefs.GetInt(_doorID, 0) == 1;
            if (savedAsOpen)
            {
                _anim.SetBool(_doorState, true);
                _onDoorOpened.Invoke();
            }
        }
    }
}