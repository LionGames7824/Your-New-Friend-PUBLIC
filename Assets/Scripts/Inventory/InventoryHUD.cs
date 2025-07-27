using System;
using Unity.VisualScripting.Antlr3.Runtime;
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
            InventoryManager.Instance.OnInventoryUpdated += UpdateDeletedItem;
            InventoryManager.Instance.OnItemRemoved += HandleItemRemoved;
            InventoryManager.Instance.OnItemEquipped += HandleItemEquipped;

            _btnCloseInventory.onClick.AddListener(OnExitClick);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameStateManager.Instance.IsOpenInventory -= OpenInventory;
            InventoryManager.Instance.OnItemRemoved -= HandleItemRemoved;
            InventoryManager.Instance.OnItemEquipped += HandleItemEquipped;
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
                    bool itemFound = false;

                    // Check if an existing view already represents this item
                    foreach (Transform child in _containerInventory)
                    {
                        var itemView = child.GetComponent<InventoryItemView>();
                        if (itemView != null && itemView.ItemId == item.ItemName)
                        {
                            itemView.UpdateData(item);
                            itemFound = true;
                            break;
                        }
                    }

                    // If no view exists for this item, create a new one
                    if (!itemFound)
                    {
                        GameObject itemPool = _pool.GetFromPool();
                        var itemView = itemPool.GetComponent<InventoryItemView>();
                        itemView.UpdateData(item);
                        itemPool.SetActive(true);
                    }
                }
            }
            else
            {
                _pool.ReturnAllPooledObjects(pooledItemGameObj =>
                {
                    var itemView = pooledItemGameObj.GetComponent<InventoryItemView>();
                    itemView.ResetData();
                });
            }
        }

        private void UpdateDeletedItem(string itemId, int quantity)
        {
            foreach (Transform child in _containerInventory)
            {
                var itemView = child.GetComponent<InventoryItemView>();
                if (itemView != null && itemView.ItemId == itemId)
                {
                    if (quantity > 0)
                    {
                        // Update the quantity of the existing item view
                        itemView.UpdateData(new InventoryItem { ItemName = itemId, Quantity = quantity });
                    }
                    else
                    {
                        // If the quantity is 0, remove the item from the pool and reset data
                        _pool.ReturnToPool(itemView.gameObject);
                        itemView.ResetData();
                    }
                    return;
                }
            }

            // If no existing item view is found and quantity > 0, add a new item view
            if (quantity > 0)
            {
                GameObject itemPool = _pool.GetFromPool();
                var newItemView = itemPool.GetComponent<InventoryItemView>();
                newItemView.UpdateData(new InventoryItem { ItemName = itemId, Quantity = quantity });
                itemPool.SetActive(true);
            }
        }

        private void HandleItemRemoved(string itemId)
        {
            foreach (Transform child in _containerInventory)
            {
                var itemView = child.GetComponent<InventoryItemView>();
                if (itemView != null && itemView.ItemId == itemId)
                {
                    // Use the pool's ReturnToPool method
                    _pool.ReturnToPool(itemView.gameObject);
                    itemView.ResetData();
                    break;
                }
            }
        }

        private void HandleItemEquipped(string itemId)
        {
            gameObject.SetActive(false);
            GameStateManager.Instance.IsOpenInventory(false);
        }
    }
}