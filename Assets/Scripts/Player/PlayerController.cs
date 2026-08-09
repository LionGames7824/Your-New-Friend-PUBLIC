using System;
using UnityEngine;

namespace YourNewFriend
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera Control Variables")]
        [SerializeField] private Transform _cameraLookAt;
        [SerializeField] private float _minPitch;
        [SerializeField] private float _maxPitch;
        [SerializeField] private float _minYaw;
        [SerializeField] private float _maxYaw;

        [Header("Player Movement Variables")]
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravityValue = -9.81f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _playerAcceleratedSpeed = 4f;
        [SerializeField] private float _playerNormalSpeed = 2f;
        [SerializeField] private float _playerCrouchSpeed = 1f;

        [Header("Player Hand Variables")]
        [SerializeField] private GameObject _playerHandObject;

        [Header("Interaction with Objects")]
        [SerializeField] private InteractionDetector _interactionDetector;

        [Header("Player Sound Effects")]
        [SerializeField] private string _playerWalking;
        [SerializeField] private float _footstepInterval = 0.5f;
        [SerializeField] private string _playerJumpStart;
        [SerializeField] private string _playerJumpEnd;
        [SerializeField] private string _playerGrabbingItem;
        [SerializeField] private string _playerDroppingItem;
        private float _footstepTimer = 0f;

        [Header("Player Spawn Positions")]
        [SerializeField] private GameObject[] _spawn;

        // Player State Variables
        private float _playerSpeed;
        private float _currentCameraYaw;
        private float _currentCameraPitch;
        private PlayerInput _playerInput;
        private CharacterController _controller;
        private Animator _anim;
        private Transform _cameraMainTransform;
        private Vector3 _playerVelocity;
        private Vector2 _inputVector;
        private bool _groundedPlayer;
        private bool _wasGrounded;
        private bool _canControl = true;
        private Transform _playerPosition;

        private void Awake()
        {
            _playerPosition = GetComponent<Transform>();
            _controller = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            _anim = GetComponent<Animator>();
            _cameraMainTransform = Camera.main.transform;
            _playerSpeed = _playerNormalSpeed;

            _playerInput.OnPlayerMove += OnPlayerMove;
            _playerInput.OnPlayerJump += OnPlayerJump;
            _playerInput.OnPlayerInteract += OnPlayerInteract;
            _playerInput.OnPlayerCamera += UpdateCameraRotation;
            _playerInput.OnPlayerSprint += OnPlayerSprint;
            _playerInput.OnPlayerCrouch += OnPlayerCrouch;
            _playerInput.OnPlayerDropItem += OnPlayerDropItem;
            _playerInput.OnPlayerUnequipItem += OnPlayerUnequipItem;

            Cover.onOpenCover += HandleCoverOpen;
            GameStateManager.Instance.IsOpenInventory += OnInventoryToggled;
            GameStateManager.Instance.IsOpenSettings += OnSettingsToggled;

            InitializeCameraRotation();

            _playerPosition.position = _spawn[PlayerPrefs.GetInt("POS_ID")].transform.position;
            Debug.Log("Position Loaded: " + _playerPosition.position);
        }

        private void HandleCoverOpen(bool state)
        {
            SetControl(state);
        }

        private void OnDestroy()
        {
            _playerInput.OnPlayerMove -= OnPlayerMove;
            _playerInput.OnPlayerJump -= OnPlayerJump;
            _playerInput.OnPlayerInteract -= OnPlayerInteract;
            _playerInput.OnPlayerCamera -= UpdateCameraRotation;
            _playerInput.OnPlayerSprint -= OnPlayerSprint;
            _playerInput.OnPlayerCrouch -= OnPlayerCrouch;
            _playerInput.OnPlayerDropItem -= OnPlayerDropItem;
            _playerInput.OnPlayerUnequipItem -= OnPlayerUnequipItem;

            GameStateManager.Instance.IsOpenInventory -= OnInventoryToggled;
        }

        public void LoadPlayerPosition()
        {
            _playerPosition.position = _spawn[PlayerPrefs.GetInt("POS_ID")].transform.position;
            Debug.Log("Position Loaded: " + _playerPosition.position);
        }

        private void Update()
        {
            MovePlayer();
            UpdatePlayerRotation();
        }

        public void SetControl(bool canControl)
        {
            _canControl = canControl;
        }

        // --- Camera ---
        private void InitializeCameraRotation()
        {
            Vector3 lookDirection = (_cameraLookAt.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            _currentCameraYaw = transform.eulerAngles.y;
            _currentCameraPitch = transform.eulerAngles.x;
        }

        private void UpdateCameraRotation(Vector2 posDelta)
        {
            if (!_canControl) return;

            _currentCameraYaw += posDelta.x * _rotationSpeed * Time.deltaTime;
            _currentCameraPitch -= posDelta.y * _rotationSpeed * Time.deltaTime;
            _currentCameraPitch = Mathf.Clamp(_currentCameraPitch, _minPitch, _maxPitch);

            transform.rotation = Quaternion.Euler(0, _currentCameraYaw, 0);
            _cameraMainTransform.localRotation = Quaternion.Euler(_currentCameraPitch, 0, 0);
        }

        private void UpdatePlayerRotation()
        {
            transform.rotation = Quaternion.Euler(0, _currentCameraYaw, 0);
        }

        // --- Movement ---
        private void MovePlayer()
        {
            UpdateGroundState();
            ApplyMovement();
            ApplyRotation();
            ApplyGravity();
            PlayFootstepSounds();
        }

        private void UpdateGroundState()
        {
            _groundedPlayer = _controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 0.1f);

            if (_groundedPlayer && _playerVelocity.y < 0)
                _playerVelocity.y = -2f;
        }

        private void ApplyMovement()
        {
            Vector3 moveDirection = new Vector3(_inputVector.x, 0, _inputVector.y);
            moveDirection = _cameraMainTransform.forward * moveDirection.z + _cameraMainTransform.right * moveDirection.x;
            moveDirection.y = 0f;

            _controller.Move(moveDirection * Time.deltaTime * _playerSpeed);
        }

        private void ApplyRotation()
        {
            if (_inputVector == Vector2.zero) return;

            float targetAngle = Mathf.Atan2(_inputVector.x, _inputVector.y) * Mathf.Rad2Deg + _cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        }

        private void ApplyGravity()
        {
            const float fallMultiplier = 2.0f;
            if (!_groundedPlayer || _playerVelocity.y > 0)
                _playerVelocity.y += _gravityValue * fallMultiplier * Time.deltaTime;

            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        private void PlayFootstepSounds()
        {
            if (_inputVector != Vector2.zero && _groundedPlayer)
            {
                _footstepTimer -= Time.deltaTime;
                if (_footstepTimer <= 0f)
                {
                    AudioManager.Instance.PlaySFX(_playerWalking);
                    _footstepTimer = _footstepInterval;
                }
            }
            else
            {
                _footstepTimer = 0f;
            }
        }

        // --- Input Events ---
        private void OnPlayerMove(Vector2 vector)
        {
            if (!_canControl) return;
            _inputVector = vector;
        }

        private void OnPlayerSprint(float value)
        {
            if (!_canControl) return;
            _playerSpeed = value > 0 ? _playerAcceleratedSpeed : _playerNormalSpeed;
        }

        private void OnPlayerCrouch(float value)
        {
            if (!_canControl) return;

            if (value > 0)
            {
                _anim.SetTrigger("Crouch");
                _playerSpeed = _playerCrouchSpeed;
            }
            else
            {
                _anim.SetTrigger("Stand");
                _playerSpeed = _playerNormalSpeed;
            }
        }

        private void OnPlayerInteract(float value)
        {
            Debug.Log($"[INPUT] OnPlayerInteract called, value={value}");
            if (!_canControl) return;
            if (value <= 0) return;

            if (_interactionDetector.TryInteract(this))
                AudioManager.Instance.PlaySFX(_playerGrabbingItem);
        }

        private void OnPlayerJump(float value)
        {
            if (!_canControl) return;

            bool isGroundedNow = _controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 0.1f);

            if (value > 0 && isGroundedNow)
            {
                _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);
                AudioManager.Instance.PlaySFX(_playerJumpStart);
            }

            if (!_wasGrounded && isGroundedNow)
            {
                AudioManager.Instance.PlaySFX(_playerJumpEnd);
            }

            _wasGrounded = isGroundedNow;
        }

        private void OnPlayerDropItem()
        {
            Transform itemEquipped = _playerHandObject.transform.childCount > 0 ? _playerHandObject.transform.GetChild(0) : null;
            if (itemEquipped != null)
            {
                InventoryManager.Instance.DropItemEquipped();
            }
        }

        private void OnPlayerUnequipItem()
        {
            InventoryManager.Instance.UnequipItem();
        }

        private void OnInventoryToggled(bool isOpen)
        {
            if (!isOpen)
            {
                _playerInput.ResumeGame();
                SetControl(true);
            }
            else
            {
                bool isHoldingItem = !string.IsNullOrEmpty(InventoryManager.Instance._itemEquipped);
                if (!isHoldingItem)
                    SetControl(false);
            }
        }

        private void OnSettingsToggled(bool isOpen)
        {
            if (!isOpen)
            {
                _playerInput.ResumeGame();
            }
        }
    }
}