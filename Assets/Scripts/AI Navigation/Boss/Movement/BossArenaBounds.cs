using UnityEngine;

namespace YourNewFriend
{
    public class BossArenaBounds : MonoBehaviour
    {
        [Header("Arena Size")]
        [SerializeField] private Vector2 arenaSize = new Vector2(20f, 20f);

        public Vector3 ClampPosition(Vector3 position)
        {
            float halfWidth = arenaSize.x * 0.5f;
            float halfLength = arenaSize.y * 0.5f;

            position.x = Mathf.Clamp(
                position.x,
                transform.position.x - halfWidth,
                transform.position.x + halfWidth);

            position.z = Mathf.Clamp(
                position.z,
                transform.position.z - halfLength,
                transform.position.z + halfLength);

            return position;
        }

        // Clamps a CENTER point so that an entire circle of the given radius
        // around it stays inside the arena box, instead of clamping the
        // circle's edge point itself (which cuts corners flat and can leave
        // a mover with no reachable target near the walls). Kept around in
        // case an orbit-style behaviour comes back later.
        public Vector3 ClampCenterForRadius(Vector3 position, float radius)
        {
            float halfWidth = Mathf.Max(arenaSize.x * 0.5f - radius, 0f);
            float halfLength = Mathf.Max(arenaSize.y * 0.5f - radius, 0f);

            position.x = Mathf.Clamp(
                position.x,
                transform.position.x - halfWidth,
                transform.position.x + halfWidth);

            position.z = Mathf.Clamp(
                position.z,
                transform.position.z - halfLength,
                transform.position.z + halfLength);

            return position;
        }

        // A random point strictly inside the arena box at the given height.
        // Always inside by construction - nothing needs to be clamped
        // afterwards, so there's no risk of a mover getting stuck chasing an
        // edge/corner point the way clamping a moving target can.
        public Vector3 GetRandomPoint(float height)
        {
            float halfWidth = arenaSize.x * 0.5f;
            float halfLength = arenaSize.y * 0.5f;

            float x = Random.Range(transform.position.x - halfWidth, transform.position.x + halfWidth);
            float z = Random.Range(transform.position.z - halfLength, transform.position.z + halfLength);

            return new Vector3(x, height, z);
        }

        public bool IsInsideArena(Vector3 position)
        {
            float halfWidth = arenaSize.x * 0.5f;
            float halfLength = arenaSize.y * 0.5f;

            return position.x >= transform.position.x - halfWidth &&
                   position.x <= transform.position.x + halfWidth &&
                   position.z >= transform.position.z - halfLength &&
                   position.z <= transform.position.z + halfLength;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawWireCube(
                transform.position,
                new Vector3(arenaSize.x, 0.1f, arenaSize.y));
        }
    }
}
