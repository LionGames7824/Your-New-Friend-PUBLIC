using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YourNewFriend
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        private const string _inventorySaveKey = "InventorySave";

        public Inventory Inventory => _inventory;
        public event Action<string, int> OnInventoryUpdated;
        public event Action<string> OnItemRemoved;
        public event Action<string> OnItemDropped;
        public event Action<string> OnItemEquipped;
        public event Action OnDropItemEquipped;

        private Inventory _inventory;

        private String _itemEquipped = string.Empty;

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

        public void AddItem(string itemId, int quantity)
        {
            var inventoryItem = _inventory.AddItem(itemId, quantity);
            SaveInventory();
            OnInventoryUpdated?.Invoke(itemId, inventoryItem != null ? inventoryItem.Quantity : quantity);
        }

        public void RemoveItem(string itemId, int quantity)
        {
            InventoryItem item = _inventory.Items.Find(i => i.ItemName == itemId);
            if (item != null)
            {
                item.Quantity -= quantity;
                if (item.Quantity <= 0)
                {
                    _inventory.Items.Remove(item);
                    OnInventoryUpdated?.Invoke(itemId, 0);
                    OnItemRemoved?.Invoke(itemId);
                }
                else
                {
                    OnInventoryUpdated?.Invoke(itemId, item.Quantity);
                }
                SaveInventory();
                OnItemDropped?.Invoke(itemId);
            }
        }

        public void EquipItem(string itemId)
        {
            _itemEquipped = itemId;
            OnItemEquipped?.Invoke(itemId);
        }

        public void DropItemEquipped()
        {
            OnDropItemEquipped?.Invoke();
            RemoveItem(_itemEquipped, 1);
            _itemEquipped = string.Empty;
        }

        public bool CheckItem(string itemId)
        {
            InventoryItem item = _inventory.Items.Find(i => i.ItemName == itemId);
            return item != null;
        }

        public int CheckQuantity(string itemId)
        {
            InventoryItem item = _inventory.Items.Find(i => i.ItemName == itemId);
            return item != null ? item.Quantity : 0;
        }        
    }
}
