using UnityEngine;
using UnityEngine.UI;

namespace YourNewFriend
{
    public class InventoryHUD : MonoBehaviour
    {
        [SerializeField] private Button _btnCloseInventory;
        [SerializeField] private GameObject _inventoryScreen;
        void Start()
        {
            _btnCloseInventory.onClick.AddListener(OnExitClick);
        }

        private void OnExitClick()
        {
            _inventoryScreen.SetActive(false);
            GameStateManager.Instance.IsOpenInventory(false);
        }
    }
}