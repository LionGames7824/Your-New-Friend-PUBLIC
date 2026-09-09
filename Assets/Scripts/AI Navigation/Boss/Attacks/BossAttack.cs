using System;
using UnityEngine;

namespace YourNewFriend
{
    public class BossAttack : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _laserOrigin;
        [SerializeField] private LineRenderer _laser;

        [Header("Laser")]
        [SerializeField] private float _laserDuration = 2f;
        [SerializeField] private float _laserHeight = 10f;
        [SerializeField] private float _damage = 10f;

        [Header("Laser Visuals")]
        [Tooltip("Base thickness of the beam once fully grown in.")]
        [SerializeField] private float _baseWidth = 0.25f;
        [Tooltip("How long the beam takes to grow from 0 to full width when it starts firing.")]
        [SerializeField] private float _growInTime = 0.12f;
        [Tooltip("How fast the width pulses while firing, so the beam doesn't look perfectly static.")]
        [SerializeField] private float _pulseSpeed = 14f;
        [Tooltip("How much the width pulses, as a fraction of the base width (0.15 = +/-15%).")]
        [SerializeField] private float _pulseAmount = 0.15f;
        [Tooltip("How many points the beam is built from. More points = smoother jitter/curve.")]
        [SerializeField] private int _segmentCount = 10;
        [Tooltip("How far each point can wobble sideways off the straight line, for a crackling energy look.")]
        [SerializeField] private float _jitterAmount = 0.08f;

        private bool _isAttacking;
        private float _attackElapsed;
        private const string _playerTag = "Player";

        public bool IsAttacking => _isAttacking;
        public event Action OnAttackFinished;

        private void Awake()
        {
            if (_player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag(_playerTag);
                if (playerObject != null)
                    _player = playerObject.transform;
            }

            if (_laser != null)
                _laser.enabled = false;
        }

        private void Update()
        {
            if (!_isAttacking)
                return;

            _attackElapsed += Time.deltaTime;
            UpdateLaserVisuals();
        }

        public void StartLaser()
        {
            if (_isAttacking || _player == null || _laserOrigin == null)
                return;

            _isAttacking = true;
            _attackElapsed = 0f;

            if (_laser != null)
            {
                _laser.enabled = true;
                UpdateLaserVisuals();
            }

            Invoke(nameof(StopLaser), _laserDuration);
        }

        // Rebuilds the beam every frame while firing:
        // - width grows in quickly then pulses gently instead of snapping
        //   straight to full size and staying perfectly static.
        // - the beam is built from several points wobbled sideways (never
        //   at the very start/end, so it still lands exactly on the
        //   origin and the player) instead of a single rigid straight
        //   line, for a crackling/energetic look with no extra shader or
        //   texture needed.
        private void UpdateLaserVisuals()
        {
            if (_laser == null)
                return;

            Vector3 origin = _laserOrigin.position;
            Vector3 endPoint = new Vector3(origin.x, _player.position.y, origin.z);

            float growIn = _growInTime > 0f ? Mathf.Clamp01(_attackElapsed / _growInTime) : 1f;
            float pulse = 1f + Mathf.Sin(_attackElapsed * _pulseSpeed) * _pulseAmount;
            _laser.widthMultiplier = _baseWidth * growIn * pulse;

            int segmentCount = Mathf.Max(_segmentCount, 2);
            _laser.positionCount = segmentCount;

            Vector3 direction = (endPoint - origin).normalized;
            Vector3 sideA = Vector3.Cross(direction, Vector3.up);
            if (sideA.sqrMagnitude < 0.0001f)
                sideA = Vector3.Cross(direction, Vector3.right);
            sideA.Normalize();
            Vector3 sideB = Vector3.Cross(direction, sideA);

            for (int i = 0; i < segmentCount; i++)
            {
                float t = i / (float)(segmentCount - 1);
                Vector3 point = Vector3.Lerp(origin, endPoint, t);

                // Fades to zero at both ends (t=0 and t=1) so the beam
                // always starts exactly at the origin and lands exactly on
                // the player, only wobbling in between.
                float edgeFade = Mathf.Sin(t * Mathf.PI);
                Vector3 jitter =
                    (sideA * UnityEngine.Random.Range(-1f, 1f) +
                     sideB * UnityEngine.Random.Range(-1f, 1f)) * _jitterAmount * edgeFade;

                _laser.SetPosition(i, point + jitter);
            }
        }

        private void StopLaser()
        {
            _isAttacking = false;

            if (_laser != null)
                _laser.enabled = false;

            OnAttackFinished?.Invoke();
        }
    }
}
