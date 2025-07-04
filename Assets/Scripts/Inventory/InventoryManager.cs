using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YourNewFriend
{
    [Serializable]
    public class ItemPrefabMap
    {
        public string ItemId;
        public GameObject ItemPrefab;
    }

    public class InventoryManager : Singleton<InventoryManager>
    {
        private const string _inventorySaveKey = "InventorySave";

        public Inventory Inventory => _inventory;
        public event Action<string, int> OnInventoryUpdated;
        public event Action<string> OnItemRemoved;

        [Header("Item Drop Settings")]
        [SerializeField] private List<ItemPrefabMap> _itemPrefabs;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _spawnDistance = 1.5f;
        [SerializeField] private Vector3 _spawnOffset = new Vector3(0, 0.5f, 0);

        private Inventory _inventory;

        private void Start()
        {
            LoadInventory();
        }

        public void SaveInventory()
        {
            if (PersistenceDataHelper.TryPersistData(_inventorySaveKey, _inventory))
            {
                Debug.Log("Inventory saved!");
            }
        }

        public void LoadInventory()
        {
            if (PersistenceDataHelper.TryLoadPersistedData<Inventory>(_inventorySaveKey, out _inventory))
            {
                Debug.Log("Inventory loaded!");
            }
            else
            {
                Debug.Log("No inventory file found, creating a new one.");
                _inventory = new Inventory();
            }
        }

        public void AddItem(string itemName, int quantity)
        {
            var inventoryItem = _inventory.AddItem(itemName, quantity);
            SaveInventory();
            OnInventoryUpdated?.Invoke(itemName, inventoryItem != null ? inventoryItem.Quantity : quantity);
        }

        public void RemoveItem(string itemName, int quantity)
        {
            InventoryItem item = _inventory.Items.Find(i => i.ItemName == itemName);
            if (item != null)
            {
                item.Quantity -= quantity;
                if (item.Quantity <= 0)
                {
                    _inventory.Items.Remove(item);
                    OnInventoryUpdated?.Invoke(itemName, 0);
                }
                else
                {
                    OnInventoryUpdated?.Invoke(itemName, item.Quantity);
                }

                SaveInventory();
                SpawnDroppedItem(itemName);
            }
        }

        public bool CheckItem(string itemName)
        {
            InventoryItem item = _inventory.Items.Find(i => i.ItemName == itemName);
            return item != null;
        }

        public int CheckQuantity(string itemName)
        {
            InventoryItem item = _inventory.Items.Find(i => i.ItemName == itemName);
            return item != null ? item.Quantity : 0;
        }

        private void SpawnDroppedItem(string itemName)
        {
            Debug.Log($"Attempting to drop item: {itemName}");

            GameObject prefab = _itemPrefabs.Find(map => map.ItemId == itemName)?.ItemPrefab;
            if (prefab == null)
            {
                Debug.LogWarning($"Item '{itemName}' has a null prefab assigned.");
                return;
            }

            Vector3 forward = _playerTransform.forward;
            Vector3 spawnPos = _playerTransform.position + forward * _spawnDistance + _spawnOffset;

            Debug.Log($"Spawning '{itemName}' at {spawnPos}");
            GameObject droppedItem = GameObject.Instantiate(prefab, spawnPos, Quaternion.identity);
            droppedItem.name = $"Dropped_{itemName}";
        }

    }
}
