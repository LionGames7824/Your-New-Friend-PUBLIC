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

        [Header("Wander")]
        [Tooltip("How close to the current target before picking a new random point.")]
        [SerializeField] private float _arrivalDistance = 0.5f;

        private Vector3 _wanderTarget;
        private bool _canMove = true;

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

            ChooseWanderPoint();
        }

        private void Update()
        {
            if (_player == null)
                return;

            if (!_canMove)
            {
                transform.LookAt(_player);
                return;
            }

            Wander();
            transform.LookAt(_player);
        }

        // Just flies from one random point inside the arena to the next,
        // forever. No orbiting the player, no clamping a moving target
        // against the walls - every waypoint is generated already inside
        // the bounds (see BossArenaBounds.GetRandomPoint), so there's
        // nothing for the boss to get stuck against near a corner.
        private void Wander()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _wanderTarget,
                _moveSpeed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, _wanderTarget);

            if (distance <= _arrivalDistance)
            {
                ChooseWanderPoint();
            }
        }

        private void ChooseWanderPoint()
        {
            float height = GetHoverHeight();

            _wanderTarget = _arenaBounds != null
                ? _arenaBounds.GetRandomPoint(height)
                : transform.position;
        }

        private float GetHoverHeight()
        {
            // Anchored to the arena itself (not the player's current Y) so
            // the hover height stays steady even if the player jumps or
            // stands on uneven ground.
            float baseHeight = _arenaBounds != null ? _arenaBounds.transform.position.y : transform.position.y;
            return baseHeight + _hoverHeight;
        }

        // Names kept as-is (StartOrbit/StopOrbit) so BossController doesn't
        // need to change - they now just start/stop the wander behaviour.
        public void StartOrbit()
        {
            ChooseWanderPoint();
            _canMove = true;
        }

        public void StopOrbit()
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
