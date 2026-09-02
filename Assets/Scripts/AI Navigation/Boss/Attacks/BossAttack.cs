using System;
using UnityEngine;

namespace YourNewFriend
{
    public class BossAttack : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform laserOrigin;
        [SerializeField] private LineRenderer laser;

        [Header("Laser")]
        [SerializeField] private float laserDuration = 2f;
        [SerializeField] private float laserHeight = 10f; // altura acima do alvo
        [SerializeField] private float damage = 10f;

        private bool isAttacking;
        private Vector3 lockedTargetPos;

        public bool IsAttacking => isAttacking;
        public event Action OnAttackFinished;

        private void Awake()
        {
            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                    player = playerObject.transform;
            }

            if (laser != null)
                laser.enabled = false;
        }

        public void StartLaser()
        {
            if (isAttacking || player == null || laserOrigin == null)
                return;

            isAttacking = true;

            if (laser != null)
            {
                laser.enabled = true;
                AimLaser(); // já desenha a primeira posição correta
            }

            Invoke(nameof(StopLaser), laserDuration);
        }

        private void AimLaser()
        {
            Vector3 origin = laserOrigin.position;
            Vector3 endPoint = new Vector3(origin.x, player.position.y, origin.z); // desce reto, sem desviar em X/Z

            laser.SetPosition(0, origin);
            laser.SetPosition(1, endPoint);
        }

        private void StopLaser()
        {
            isAttacking = false;

            if (laser != null)
                laser.enabled = false;

            OnAttackFinished?.Invoke();
        }
    }
}