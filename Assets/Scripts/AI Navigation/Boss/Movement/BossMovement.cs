using UnityEngine;

namespace YourNewFriend
{
    public class BossMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float hoverHeight = 3f;
        private bool canMove = true;

        [Header("Orbit")]
        [SerializeField] private float orbitRadius = 5f;
        [SerializeField] private float orbitSpeed = 40f;

        private Vector3 orbitTarget;
        private float orbitAngle;
        private bool isOrbiting;

        private void Awake()
        {
            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }

            ChooseOrbitPoint();
        }

        private void Update()
        {
            if (player == null)
                return;

            if (!canMove)
            {
                transform.LookAt(player);
                return;
            }

            if (!isOrbiting)
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
                orbitTarget,
                moveSpeed * Time.deltaTime);

            float distance = Vector3.Distance(
                transform.position,
                orbitTarget);

            if (distance <= 0.2f)
            {
                isOrbiting = true;
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
            ) * orbitRadius;

            orbitTarget =
                player.position +
                Vector3.up * hoverHeight +
                offset;

            orbitAngle = angle;
        }

        private void OrbitPlayer()
        {
            orbitAngle += orbitSpeed * Time.deltaTime;

            float radians = orbitAngle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(radians),
                0f,
                Mathf.Sin(radians)
            ) * orbitRadius;

            Vector3 targetPosition =
                player.position +
                Vector3.up * hoverHeight +
                offset;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);
        }

        public void StopOrbit()
        {
            isOrbiting = false;
        }

        public void StartOrbit()
        {
            isOrbiting = true;
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