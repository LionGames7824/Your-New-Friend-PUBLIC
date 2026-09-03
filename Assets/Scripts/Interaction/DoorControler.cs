using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class DoorController : MonoBehaviour
    {
        public enum DoorLockState
        {
            OffGreen,  
            OffRed,    
            Green,     
            Red        
        }

        [Header("Persistence")]
        [Tooltip("Automatically generated. If you need to reset it, delete the text and save the scene.")]
        [SerializeField] private string _doorID;

        [Header("Settings")]
        [SerializeField] private bool _startOpened = false;
        [SerializeField] private DoorLockState _lockState = DoorLockState.OffGreen;
        [SerializeField] private bool _resetOnGameOver = false;

        [Header("References")]
        [SerializeField] private PanelLightController _panelLight;

        [Header("Events")]
        [SerializeField] private UnityEvent _onDoorOpened;

        private Animator _anim;
        private const string DOOR_STATE = "Door_State";

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        private void Start()
        {
            ListGeneration.doors.Add(gameObject);
            ApplyLockState();
            LoadDoorState();
        }

        // --- Lock State ---

        public void SetLockState(DoorLockState newState)
        {
            _lockState = newState;
            ApplyLockState();
        }

        private void ApplyLockState()
        {
            if (_panelLight == null) return;

            switch (_lockState)
            {
                case DoorLockState.OffGreen:
                    _panelLight.SetLight(PanelLightState.OffGreen);
                    break;
                case DoorLockState.OffRed:
                    _panelLight.SetLight(PanelLightState.OffRed);
                    break;
                case DoorLockState.Green:
                    _panelLight.SetLight(PanelLightState.Green);
                    break;
                case DoorLockState.Red:
                    _panelLight.SetLight(PanelLightState.Red);
                    break;
            }
        }

        // --- Light onn / off ---

        public void TurnOnLight()
        {
            switch (_lockState)
            {
                case DoorLockState.OffGreen:
                    SetLockState(DoorLockState.Green);
                    break;
                case DoorLockState.OffRed:
                    SetLockState(DoorLockState.Red);
                    break;
            }
        }

        public void TurnOffLight()
        {
            switch (_lockState)
            {
                case DoorLockState.Green:
                    SetLockState(DoorLockState.OffGreen);
                    break;
                case DoorLockState.Red:
                    SetLockState(DoorLockState.OffRed);
                    break;
            }
        }

        public void SetGreen()
        {
            SetLockState(DoorLockState.Green);
        }

        public void SetRed()
        {
            SetLockState(DoorLockState.Red);
        }

        // --- Open Door ---

        public void TryOpenWithCard()
        {
            if (_lockState != DoorLockState.Green) return;
            OpenDoor();
        }

        public void TryOpenWithForce()
        {
            if (_lockState != DoorLockState.Red) return;
            OpenDoor();
        }

        private void OpenDoor()
        {
            if (_anim.GetBool(DOOR_STATE)) return;

            _anim.SetBool(DOOR_STATE, true);
            SaveDoorState(true);
            _onDoorOpened.Invoke();            
        }

        public void ToggleDoor()
        {
            bool isOpening = !_anim.GetBool(DOOR_STATE);
            _anim.SetBool(DOOR_STATE, isOpening);
            SaveDoorState(isOpening);
            if (isOpening) _onDoorOpened.Invoke();
        }

        // --- Persistence ---

        private void SaveDoorState(bool open)
        {
            if (string.IsNullOrEmpty(_doorID)) return;
            if (_resetOnGameOver == false)
            {
                Debug.Log($"DOOR WAS SAVED: {_doorID} " + open);
                PlayerPrefs.SetInt(_doorID, open ? 1 : 0);
                PlayerPrefs.Save();
            }
            return;
        }

        private void LoadDoorState()
        {
            if (string.IsNullOrEmpty(_doorID)) return;

            bool savedAsOpen = PlayerPrefs.HasKey(_doorID)
                ? PlayerPrefs.GetInt(_doorID, 0) == 1
                : _startOpened;

            if (savedAsOpen)
            {
                _anim.SetBool(DOOR_STATE, true);
                _onDoorOpened.Invoke();
            }
            else
            {
                _anim.SetBool(DOOR_STATE, false);
            }
        }

        public static void LoadAllDoorsState()
        {
            foreach (var door in ListGeneration.doors)
            {
                var doorController = door.GetComponent<DoorController>();
                if (doorController != null)
                {
                    Debug.Log("All the doors are loaded");
                    doorController.LoadDoorState();
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_doorID))
            {
                _doorID = System.Guid.NewGuid().ToString();
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
                Debug.Log($"ID generated for {gameObject.name}: {_doorID}");
            }
        }
#endif
    }
}