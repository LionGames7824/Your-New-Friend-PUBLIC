using UnityEngine;
using UnityEngine.InputSystem;

namespace YourNewFriend
{
    public class BossDebugDamage : MonoBehaviour
    {
        [SerializeField] private int damage = 10;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!Mouse.current.leftButton.wasPressedThisFrame)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Acertou: " + hit.collider.name);

                BossHealth bossHealth = hit.collider.GetComponentInParent<BossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(damage);
                }
            }
            else
            {
                Debug.Log("Não acertou nada");
            }
        }
    }
}