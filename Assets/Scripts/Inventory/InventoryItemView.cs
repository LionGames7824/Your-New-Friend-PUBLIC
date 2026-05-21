using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YourNewFriend
{
    public class InventoryItemView : MonoBehaviour
    {
        [System.Serializable]
        public class IconMap
        {
            public string ItemId;
            public Sprite TargetSprite;
        }

        [SerializeField] private List<IconMap> _iconMapping;
        [SerializeField] private TMP_Text _quantityLabel;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _btnRemoveItem;
        [SerializeField] private Button _btnUseItem;

        public string ItemId { get; private set; }
        public Action<string> ItemUsed;

        private Sprite _defaultSprite;

        private void Awake()
        {
            _defaultSprite = _icon.sprite;
            _icon.preserveAspect = true;
        }

        public void OnRemoveItem()
        {
            InventoryManager.Instance.RemoveItem(ItemId, 1);
        }

        public void OnUseItem()
        {
            Debug.Log($"{GetType().Name}: Item Equipped {ItemId}");
            InventoryManager.Instance.EquipItem(ItemId);
        }

        public void UpdateData(InventoryItem inventoryItem)
        {
            ItemId = inventoryItem.ItemName;

            if (inventoryItem.Quantity == 1)
            {
                _quantityLabel.text = $"";
            }
            else
            {
                _quantityLabel.text = $"{inventoryItem.Quantity}";
            }

            foreach (var iconMap in _iconMapping)
            {
                if (!string.IsNullOrEmpty(inventoryItem.ItemName) && inventoryItem.ItemName.Equals(iconMap.ItemId))
                {
                    _icon.sprite = iconMap.TargetSprite;
                    break;
                }
            }
        }

        public void ResetData()
        {
            _icon.sprite = _defaultSprite;
            ItemId = string.Empty;
            _quantityLabel.text = string.Empty;
        }
    }
}
