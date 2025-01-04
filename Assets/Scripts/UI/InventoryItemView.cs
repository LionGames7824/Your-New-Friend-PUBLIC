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
            public Color TargetColor;
        }

        [SerializeField] private List<IconMap> _iconMapping;
        [SerializeField] private TMP_Text _quantityLabel;
        [SerializeField] private Image _icon;

        public string ItemId { get; private set; }

        private Color _defaultColor;

        private void Awake()
        {
            _defaultColor = _icon.color;
        }

        public void UpdateData(InventoryItem inventoryItem)
        {
            ItemId = inventoryItem.ItemName;
            _quantityLabel.text = $"{inventoryItem.Quantity}";

            foreach (var iconMap in _iconMapping)
            {
                if (!string.IsNullOrEmpty(inventoryItem.ItemName) && inventoryItem.ItemName.Equals(iconMap.ItemId))
                {
                    _icon.color = iconMap.TargetColor;
                    break;
                }
            }
        }

        public void ResetData()
        {
            _icon.color = _defaultColor;
            ItemId = string.Empty;
            _quantityLabel.text = string.Empty;
        }
    }
}
