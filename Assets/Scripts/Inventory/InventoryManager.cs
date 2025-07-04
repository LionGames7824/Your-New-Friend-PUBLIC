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
                    OnItemRemoved?.Invoke(itemName);
                }
                else
                {
                    OnInventoryUpdated?.Invoke(itemName, item.Quantity);
                }
                SaveInventory();
                OnItemDropped?.Invoke(itemName);
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
    }
}
