using System;
using UnityEngine;

namespace YourNewFriend
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Movement Variables")]
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravityValue = -9.81f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _playerAcceleratedSpeed = 4f;
        [SerializeField] private float _playerNormalSpeed = 2f;
        [SerializeField] private float _playerCrouchSpeed = 1f;

        [Header("Camera Control Variables")]
        [SerializeField] private Transform _cameraLookAt;
        [SerializeField] private float _minPitch;
        [SerializeField] private float _maxPitch;
        [SerializeField] private float _minYaw;
        [SerializeField] private float _maxYaw;

        [Header("Interaction with objects")]
        [SerializeField] private LayerMask _raycastLayers;
        [SerializeField] private LayerMask _raycastObstructionLayer;
        [SerializeField] private float _raycastDistance = 5f;
        
        [Header("Debug Tool")]
        [SerializeField] private bool _debugEnabled;

        // Player State Variables
        private float _playerSpeed;
        private float _currentCameraYaw;
        private float _currentCameraPitch;
        private bool _isInteractable;
        private PlayerInput _playerInput;
        private CharacterController _controller;
        private Transform _cameraMainTransform;
        private Vector3 _playerVelocity;
        private Vector2 _inputVector;
        private bool _groundedPlayer;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            _cameraMainTransform = Camera.main.transform;
            _playerSpeed = _playerNormalSpeed;

            _playerInput.OnPlayerMove += HandlePlayerMove;
            _playerInput.OnPlayerJump += HandlePlayerJump;
            _playerInput.OnPlayerInteract += HandlePlayerInteract;
            _playerInput.OnPlayerCamera += HandleCamera;
            _playerInput.OnPlayerSprint += HandlePlayerSprint;
            _playerInput.OnPlayerCrouch += HandlePlayerCrouch;
            _playerInput.OnPlayerDropItem += HandlePlayerDroppingItem;

            GameStateManager.Instance.IsOpenInventory += HandleInventory;

            _isInteractable = false;

            CalculateInitialCameraRotation();
        }



        private void OnDestroy()
        {
            _playerInput.OnPlayerMove -= HandlePlayerMove;
            _playerInput.OnPlayerJump -= HandlePlayerJump;
            _playerInput.OnPlayerInteract -= HandlePlayerInteract;
            _playerInput.OnPlayerCamera -= HandleCamera;
            _playerInput.OnPlayerSprint -= HandlePlayerSprint;
            _playerInput.OnPlayerCrouch -= HandlePlayerCrouch;

            GameStateManager.Instance.IsOpenInventory -= HandleInventory;
        }

        private void Update()
        {
            MovePlayer();
            UpdateRotation();
            Interaction();
        }

        /// <summary>
        /// Casts a ray from the player's camera forward to detect and interact with objects
        /// that implement the IInteractable interface. The ray only checks objects on the 
        /// specified interaction layers and ignores any that are on obstruction layers.
        /// If a valid interactable is hit and interaction is allowed, the object's Interact() 
        /// method is called. Includes optional debug visualization and logging.
        /// </summary>
        private void Interaction()
        {
            Vector3 direction = _cameraMainTransform.forward;
            Vector3 startPos = _cameraMainTransform.position;

            if (_debugEnabled)
            {
                Debug.DrawLine(startPos, startPos + (direction * _raycastDistance), Color.cyan, 3);
            }

            if (Physics.Raycast(startPos, direction, out RaycastHit hitInfo, _raycastDistance, _raycastLayers.value))
            {
                if ((_raycastObstructionLayer.value & (1 << hitInfo.collider.gameObject.layer)) != 0)
                    return;
                
                var interactable = hitInfo.collider.gameObject.GetComponent<IInteractable>();

                if (_debugEnabled)
                {
                    Debug.Log(interactable.InteractionPrompt);
                }

                if (interactable != null && _isInteractable)
                {
                    interactable.Interact(this);
                }

            }            
            _isInteractable = false;
        }

        /*
         * Set the camera to a specific position when initializing the game
         */
        private void CalculateInitialCameraRotation()
        {
            Vector3 lookDirection = (_cameraLookAt.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            _currentCameraYaw = transform.eulerAngles.y;
            _currentCameraPitch = transform.eulerAngles.x;
        }

        private void MovePlayer()
        {
            // Raycast for better ground check
            _groundedPlayer = _controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 0.1f);

            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                // Apply a small downward force to maintain ground detection
                _playerVelocity.y = -2f;
            }

            Vector3 moveNewDirection = new Vector3(_inputVector.x, 0, _inputVector.y);
            moveNewDirection = _cameraMainTransform.forward * moveNewDirection.z + _cameraMainTransform.right * moveNewDirection.x;
            moveNewDirection.y = 0f;

            _controller.Move(moveNewDirection * Time.deltaTime * _playerSpeed);

            if (_inputVector != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(_inputVector.x, _inputVector.y) * Mathf.Rad2Deg + _cameraMainTransform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
            }

            // Apply gravity
            float fallMultiplier = 2.0f;
            if (!_groundedPlayer || _playerVelocity.y > 0)
            {
                _playerVelocity.y += _gravityValue * fallMultiplier * Time.deltaTime;
            }

            _controller.Move(_playerVelocity * Time.deltaTime);
        }

        private void HandlePlayerMove(Vector2 vector)
        {
            _inputVector = vector;
        }

        private void HandlePlayerSprint(float value)
        {
            if (value > 0)
            {
                _playerSpeed = _playerAcceleratedSpeed;
            }
            else
            {
                _playerSpeed = _playerNormalSpeed;
            }
        }

        private void HandlePlayerCrouch(float value)
        {
            if (value > 0)
            {
                _controller.height = 0.1f;
                _playerSpeed = _playerCrouchSpeed;
            }
            else
            {
                _controller.height = 1.8f;
                _playerSpeed = _playerNormalSpeed;
            }
        }

        private void HandlePlayerInteract(float value)
        {            
            if (value > 0)
            {
                _isInteractable = true;
            }
        }

        private void HandlePlayerJump(float value)
        {
            _groundedPlayer = _controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 0.1f);

            if (_debugEnabled)
                Debug.Log("Value " + value + " Is grounded:" + _groundedPlayer);

            // Jump only if grounded and input is positive
            if (value > 0 && _groundedPlayer)
            {
                _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityValue);

                if (_debugEnabled)
                    Debug.Log("Player jumped. Current height: " + transform.position.y);
            }
        }

        private void HandleCamera(Vector2 posDelta)
        {
            // Calculate yaw and pitch from the input
            _currentCameraYaw += posDelta.x * _rotationSpeed * Time.deltaTime;
            _currentCameraPitch -= posDelta.y * _rotationSpeed * Time.deltaTime; 

            // Clamp the pitch to avoid excessive up/down tilt
            _currentCameraPitch = Mathf.Clamp(_currentCameraPitch, _minPitch, _maxPitch);

            // Apply the yaw rotation to the player object
            transform.rotation = Quaternion.Euler(0, _currentCameraYaw, 0);

            // Apply pitch rotation to the camera transform
            _cameraMainTransform.localRotation = Quaternion.Euler(_currentCameraPitch, 0, 0);
        }

        private void UpdateRotation()
        {
            transform.rotation = Quaternion.Euler(0, _currentCameraYaw, 0);
        }

        private void HandleInventory(bool isOpen)
        {
            if (!isOpen)
            {
                _playerInput.ResumeGame();
            }
        }

        private void HandlePlayerDroppingItem()
        {
            Debug.Log("Joguei no chao o item");
        }
    }
}