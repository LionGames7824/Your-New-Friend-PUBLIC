using System.Collections.Generic;

namespace YourNewFriend
{
    [System.Serializable]
    public class InventoryItem
    {
        public string ItemName;
        public int Quantity;
    }

    [System.Serializable]
    public class Inventory
    {
        public List<InventoryItem> Items = new();

        public InventoryItem AddItem(string itemName, int quantity)
        {
            InventoryItem item = Items.Find(i => i.ItemName == itemName);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                Items.Add(new()
                {
                    ItemName = itemName,
                    Quantity = quantity
                });
            }

            return item;
        }

        public void RemoveItem(string itemName, int quantity)
        {
            InventoryItem item = Items.Find(i => i.ItemName == itemName);
            if (item != null)
            {
                item.Quantity -= quantity;
                if (item.Quantity <= 0)
                {
                    Items.Remove(item);
                }
            }
        }
    }
}