using UnityEngine;

namespace YourNewFriend
{
    public class BossMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _player;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _hoverHeight = 3f;
        private bool canMove = true;

        [Header("Orbit")]
        [SerializeField] private float _orbitRadius = 5f;
        [SerializeField] private float _orbitSpeed = 40f;

        private Vector3 _orbitTarget;
        private float _orbitAngle;
        private bool _isOrbiting;

        private const string _playerTag = "Player";

        private void Awake()
        {
            if (_player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag(_playerTag);

                if (playerObject != null)
                {
                    _player = playerObject.transform;
                }
            }

            ChooseOrbitPoint();
        }

        private void Update()
        {
            if (_player == null)
                return;

            if (!canMove)
            {
                transform.LookAt(_player);
                return;
            }

            if (!_isOrbiting)
            {
                ApproachOrbitPoint();
            }
            else
            {
                OrbitPlayer();
            }
        }

        private void ApproachOrbitPoint()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _orbitTarget,
                _moveSpeed * Time.deltaTime);

            float distance = Vector3.Distance(
                transform.position,
                _orbitTarget);

            if (distance <= 0.2f)
            {
                _isOrbiting = true;
            }
        }

        private void ChooseOrbitPoint()
        {
            float angle = Random.Range(0f, 360f);

            float radians = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(radians),
                0f,
                Mathf.Sin(radians)
            ) * _orbitRadius;

            _orbitTarget =
                _player.position +
                Vector3.up * _hoverHeight +
                offset;

            _orbitAngle = angle;
        }

        private void OrbitPlayer()
        {
            _orbitAngle += _orbitSpeed * Time.deltaTime;

            float radians = _orbitAngle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(radians),
                0f,
                Mathf.Sin(radians)
            ) * _orbitRadius;

            Vector3 targetPosition =
                _player.position +
                Vector3.up * _hoverHeight +
                offset;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _moveSpeed * Time.deltaTime);
        }

        public void StopOrbit()
        {
            _isOrbiting = false;
        }

        public void StartOrbit()
        {
            _isOrbiting = true;
        }

        public void StopMovement()
        {
            canMove = false;
        }

        public void ResumeMovement()
        {
            canMove = true;
        }
    }
}