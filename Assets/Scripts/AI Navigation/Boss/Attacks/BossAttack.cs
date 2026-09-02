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

        private bool _isAttacking;
        private Vector3 _lockedTargetPos;
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

        public void StartLaser()
        {
            if (_isAttacking || _player == null || _laserOrigin == null)
                return;

            _isAttacking = true;

            if (_laser != null)
            {
                _laser.enabled = true;
                AimLaser(); 
            }

            Invoke(nameof(StopLaser), _laserDuration);
        }

        private void AimLaser()
        {
            Vector3 origin = _laserOrigin.position;
            Vector3 endPoint = new Vector3(origin.x, _player.position.y, origin.z); 

            _laser.SetPosition(0, origin);
            _laser.SetPosition(1, endPoint);
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