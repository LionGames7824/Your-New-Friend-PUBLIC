using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YourNewFriend;

namespace YourNewFriend
{
    [Serializable]
    public class ItemPrefabMap
    {
        public string ItemId;
        public GameObject ItemPrefab;
    }

    public class ItemWorldHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _playerHandTransform;

        [Header("Item Spawn Settings")]
        [SerializeField] private float _spawnDistance = 1.5f;
        [SerializeField] private Vector3 _spawnOffset = new Vector3(0, 0.5f, 0);

        [Header("Item Drop Settings")]
        [SerializeField] private List<ItemPrefabMap> _itemPrefabs;


        private void Awake()
        {
            InventoryManager.Instance.OnItemDropped += HandleItemDropped;
        }

        private void HandleItemDropped(String item)
        {
            SpawnDroppedItem(item);
        }

        private void SpawnDroppedItem(string itemName)
        {
            StorableItem[] storableItems = FindObjectsOfType<StorableItem>(true);

            foreach (var item in storableItems)
            {
                if (item.name.Contains(itemName) || item.GetId() == itemName)
                {
                    Debug.Log($"Reactivating existing item: {itemName}");

                    // Place it in front of player
                    Vector3 spawnPos = _playerTransform.position + _playerTransform.forward * _spawnDistance + _spawnOffset;
                    item.transform.position = spawnPos;

                    item.Reactivate();
                    return;
                }
            }

            Debug.LogWarning($"No inactive item with id '{itemName}' found to reactivate.");
        }
    }
}