using UnityEngine;
using UnityEngine.UI;

namespace YourNewFriend
{
    public class InventoryHUD : MonoBehaviour
    {
        [SerializeField] private Transform _containerInventory;
        [SerializeField] private GameObject _inventoryScreen;
        [SerializeField] private Button _btnCloseInventory;

        [Header("Pool Settings")]
        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private uint _initPoolSize;

        private ObjectPool _pool;

        private void Start()
        {
            _pool = GetComponent<ObjectPool>();
            _pool.Initialize(_buttonPrefab, _initPoolSize);

            GameStateManager.Instance.IsOpenInventory += OpenInventory;

            _btnCloseInventory.onClick.AddListener(OnExitClick);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.IsOpenInventory -= OpenInventory;
        }

        private void OnExitClick()
        {
            _inventoryScreen.SetActive(false);
            GameStateManager.Instance.IsOpenInventory(false);
        }

        private void OpenInventory(bool isOpen)
        {
            gameObject.SetActive(isOpen);

            if (isOpen)
            {
                foreach (var item in InventoryManager.Instance.Inventory.Items)
                {
                    GameObject itemPool = _pool.GetFromPool();
                    var itemView = itemPool.GetComponent<InventoryItemView>();
                    itemView.UpdateData(item);
                    itemPool.SetActive(true);
                }
            }
            else
            {
                _pool.ReturnAllPooledObjects((pooledItemGameObj) =>
                {
                    var itemView = pooledItemGameObj.GetComponent<InventoryItemView>();
                    itemView.ResetData();
                });
            }
        }
    }
}