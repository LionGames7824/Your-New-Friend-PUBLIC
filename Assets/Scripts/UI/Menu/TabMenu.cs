
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace YourNewFriend
{
    public class TabMenu : MonoBehaviour
    {
        [System.Serializable]
        public class Tab
        {
            public Button TabButton;
            public GameObject TabPanel;
        }

        [Header("Tabs")]
        [SerializeField] private List<Tab> _tabs = new();

        [Header("Tab Colors")]
        [SerializeField]
        private Color _selectedColor =
            new Color32(204, 204, 204, 255); 

        [SerializeField]
        private Color _normalColor =
            new Color32(100, 100, 100, 255);

        private void Awake()
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                int index = i;
                _tabs[i].TabButton.onClick.AddListener(
                    () => SelectTab(index)
                );
            }

            SelectTab(0);
        }

        public void SelectTab(int index)
        {
            if (index < 0 || index >= _tabs.Count)
                return;

            for (int i = 0; i < _tabs.Count; i++)
            {
                bool isSelected = i == index;

                _tabs[i].TabPanel.SetActive(isSelected);

                Image buttonImage =
                    _tabs[i].TabButton.targetGraphic as Image;

                if (buttonImage != null)
                {
                    buttonImage.color = isSelected
                        ? _selectedColor
                        : _normalColor;
                }
            }
        }

        private void OnDestroy()
        {
            foreach (Tab tab in _tabs)
            {
                if (tab.TabButton != null)
                    tab.TabButton.onClick.RemoveAllListeners();
            }
        }
    }
}