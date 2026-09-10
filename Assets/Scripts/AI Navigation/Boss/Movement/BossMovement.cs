using UnityEngine;

namespace YourNewFriend
{
    public class BossMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _player;
        [SerializeField] private BossArenaBounds _arenaBounds;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _hoverHeight = 3f;

        private bool _canMove;

        private const string PlayerTag = "Player";

        private void Awake()
        {
            if (_player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag(PlayerTag);

                if (playerObject != null)
                {
                    _player = playerObject.transform;
                }
            }

            if (_arenaBounds == null)
            {
                _arenaBounds = FindFirstObjectByType<BossArenaBounds>();
            }
        }

        private void Update()
        {
            if (_player == null)
                return;

            if (!_canMove)
            {
                FaceTowardsPlayer();
                return;
            }

            Chase();
            FaceTowardsPlayer();
        }

        private void Chase()
        {
            Vector3 targetPosition = GetChaseTarget();

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _moveSpeed * Time.deltaTime);
        }

        private Vector3 GetChaseTarget()
        {
            float height = GetHoverHeight();
            Vector3 targetPosition = new Vector3(_player.position.x, height, _player.position.z);

            return _arenaBounds != null
                ? _arenaBounds.ClampPosition(targetPosition)
                : targetPosition;
        }

        private float GetHoverHeight()
        {
            float baseHeight = _arenaBounds != null ? _arenaBounds.transform.position.y : transform.position.y;
            return baseHeight + _hoverHeight;
        }

        private void FaceTowardsPlayer()
        {
            Vector3 direction = _player.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        }

        public void StartChase()
        {
            _canMove = true;
        }

        public void StopChase()
        {
            _canMove = false;
        }

        public void StopMovement()
        {
            _canMove = false;
        }

        public void ResumeMovement()
        {
            _canMove = true;
        }
    }
}
