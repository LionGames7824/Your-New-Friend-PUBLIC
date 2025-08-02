using System;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private GameObject _playerHandTransform;

        [Header("Item Spawn Settings")]
        [SerializeField] private float _spawnDistance = 1.5f;
        [SerializeField] private Vector3 _spawnOffset = new Vector3(0, 0.5f, 0);

        [Header("Item Drop Settings")]
        [SerializeField] private List<ItemPrefabMap> _itemPrefabs;

        private void Awake()
        {
            InventoryManager.Instance.OnItemDropped += HandleItemDropped;
            InventoryManager.Instance.OnItemEquipped += HandleItemEquipped;
        }

        private void HandleItemEquipped(string itemId)
        {
            HoldItem(itemId);
        }

        private void HandleItemDropped(string itemId)
        {
            SpawnDroppedItem(itemId);
        }

        private void SpawnDroppedItem(string itemId)
        {
            StorableItem[] storableItems = FindObjectsOfType<StorableItem>(true);

            foreach (var item in storableItems)
            {
                if (item.name.Contains(itemId) || item.GetId() == itemId)
                {
                    Debug.Log($"Reactivating existing item: {itemId}");
                    item.transform.SetParent(null);

                    Rigidbody rb = item.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                        rb.detectCollisions = true;
                    }

                    // Place it in front of player
                    Vector3 spawnPos = _playerTransform.position + _playerTransform.forward * _spawnDistance + _spawnOffset;
                    item.transform.position = spawnPos;

                    item.Reactivate();
                    return;
                }
            }

            Debug.LogWarning($"No inactive item with id '{itemId}' found to reactivate.");
        }

        private void HoldItem(string itemId)
        {
            StorableItem[] storableItems = FindObjectsOfType<StorableItem>(true);

            foreach (var item in storableItems)
            {
                if (item.name.Contains(itemId) || item.GetId() == itemId)
                {
                    item.Reactivate();

                    Rigidbody rb = item.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.detectCollisions = false;
                    }

                    // Store the original world scale
                    Vector3 originalScale = item.transform.lossyScale;

                    // Parent the item to the hand
                    item.transform.SetParent(_playerHandTransform.transform, true);

                    // Reset local position and rotation to align properly
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localRotation = Quaternion.identity;
                    
             
                    return;
                }
            }
        }
    }
}