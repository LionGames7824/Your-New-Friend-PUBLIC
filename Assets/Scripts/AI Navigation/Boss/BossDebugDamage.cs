using UnityEngine;
using UnityEngine.InputSystem;

namespace YourNewFriend
{
    public class BossDebugDamage : MonoBehaviour
    {
        [SerializeField] private int _damage = 10;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!Mouse.current.leftButton.wasPressedThisFrame)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Hit: " + hit.collider.name);

                BossHealth bossHealth = hit.collider.GetComponentInParent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(_damage);
                }
            }
            else
            {
                Debug.Log("Missed");
            }
        }
    }
}