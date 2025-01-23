using System;
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

            // Notify UI about the updated inventory
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
                    OnInventoryUpdated?.Invoke(itemName, 0); // Notify UI the item is gone
                }
                else
                {
                    OnInventoryUpdated?.Invoke(itemName, item.Quantity); // Notify UI about the new quantity
                }
                SaveInventory();
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