using UnityEngine;

namespace YourNewFriend
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryScreen;

        private void Start()
        {
            _inventoryScreen.SetActive(false);

            GameStateManager.Instance.IsOpenInventory += HandleInventory;
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.IsOpenInventory -= HandleInventory;
        }

        private void HandleInventory(bool isOpen)
        {
            _inventoryScreen.SetActive(isOpen);
        }
    }
}