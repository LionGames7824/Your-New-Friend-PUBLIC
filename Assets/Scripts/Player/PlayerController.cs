using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YourNewFriend
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravityValue = -9.81f;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _playerSpeed = 2f;
        [SerializeField] private bool _debugEnabled;

        [SerializeField] private Transform _cameraLookAt;
        [SerializeField] private LayerMask _raycastLayers;
        [SerializeField] private LayerMask _raycastObstructionLayer;
        [SerializeField] private float _raycastDistance = 60f;
        [SerializeField] private float _cameraRotationSpeed = 1;
        [SerializeField] private float _minRotDelta = 0.3f;
        [SerializeField] private float _minPitch;
        [SerializeField] private float _maxPitch;
        [SerializeField] private float _minYaw;
        [SerializeField] private float _maxYaw;
        private float _currentCameraYaw;
        private float _currentCameraPitch;


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

            _playerInput.OnPlayerMove += OnPlayerMove;
            _playerInput.OnPlayerJump += OnPlayerJump;
            _playerInput.OnPlayerCamera += OnPlayerCamera;

            CalculateInitialCameraRotation();
        }

        private void OnDestroy()
        {
            _playerInput.OnPlayerMove -= OnPlayerMove;
            _playerInput.OnPlayerJump -= OnPlayerJump;
            _playerInput.OnPlayerCamera -= OnPlayerCamera;

        }

        private void Update()
        {
            MovePlayer();
            UpdateRotation();
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

        private void OnPlayerMove(Vector2 vector)
        {
            _inputVector = vector;
        }

        private void OnPlayerJump(float value)
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

        private void OnPlayerCamera(Vector2 posDelta)
        {
            // Calculate yaw and pitch from the input
            _currentCameraYaw += posDelta.x * _rotationSpeed * Time.deltaTime;
            _currentCameraPitch -= posDelta.y * _rotationSpeed * Time.deltaTime; // Invert pitch to match typical camera control

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
    }
}