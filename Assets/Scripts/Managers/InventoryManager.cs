using System;
using TMPro;
using UnityEngine;

namespace YourNewFriend
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        private const string _inventorySaveKey = "InventorySave";

        public Inventory Inventory => _inventory;

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
        }

        public void RemoveItem(string itemName, int quantity)
        {
            _inventory.RemoveItem(itemName, quantity);
            SaveInventory();
        }

        public bool CheckItem(string itemName)
        {
            InventoryItem item = _inventory.Items.Find(i => i.ItemName == itemName);
            return item != null;
        }
    }
}