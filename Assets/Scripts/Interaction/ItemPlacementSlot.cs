using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class ItemPlacementSlot : MonoBehaviour, IInteractable
    {
        [System.Serializable]
        private class Data
        {
            public bool IsPlaced;
        }

        [Header("Placement Settings")]
        [Tooltip("Unique save key for this slot - must not collide with any other slot's ID.")]
        [SerializeField] private string _slotId;
        [Tooltip("The item ID that fits here (must match the target item's StorableItem ID).")]
        [SerializeField] private string _requiredItemId;
        [SerializeField] private string _interactionPrompt = "[E] Place XXX";
        [Tooltip("Where the item snaps to. Leave empty to use this object's own transform.")]
        [SerializeField] private Transform _placementPoint;
        [SerializeField] private bool _canPickUp = false;
        [SerializeField] private bool _placeOnce = true;

        [Header("Events")]
        [Tooltip("Called right after the item is placed - e.g. to unlock a door, play a sound/animation.")]
        [SerializeField] private UnityEvent onItemPlaced;

        private Data _data;

        // Once placed, there's nothing left to prompt for.
        public string InteractionPrompt => (_data != null && _data.IsPlaced) ? "" : _interactionPrompt;

        private void Awake()
        {
            LoadData();
        }

        private void Start()
        {
            if (_data.IsPlaced)
                StartCoroutine(ReapplyPlacementNextFrame());
        }

        private IEnumerator ReapplyPlacementNextFrame()
        {
            yield return null;
            SnapItemIntoSlot();
        }

        private void LoadData()
        {
            if (!PersistenceDataHelper.TryLoadPersistedData(_slotId, out _data))
                _data = new Data { IsPlaced = false };
        }

        private void SaveData()
        {
            PersistenceDataHelper.TryPersistData(_slotId, _data);
        }

        public bool Interact(PlayerController interactor)
        {
            if (_data.IsPlaced) return false;

            if (InventoryManager.Instance._itemEquipped != _requiredItemId)
            {
                Debug.Log($"{GetType().Name}: precisa estar segurando '{_requiredItemId}' para colocar aqui.");
                return false;
            }

            if (_placeOnce && !_canPickUp)
            {
                _data.IsPlaced = true;
            }
            SaveData();

            InventoryManager.Instance.ConsumeEquippedItem();

            SnapItemIntoSlot();

            onItemPlaced?.Invoke();
            return true;
        }

        private void SnapItemIntoSlot()
        {
            StorableItem[] storableItems = FindObjectsOfType<StorableItem>(true);
            foreach (var item in storableItems)
            {
                if (item.GetId() != _requiredItemId) continue;

                item.Reactivate();

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.detectCollisions = _canPickUp;
                }

                Transform target = _placementPoint != null ? _placementPoint : transform;
                item.transform.SetParent(target, true);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                item.transform.localScale = Vector3.one;
                return;
            }

            Debug.LogWarning($"{GetType().Name}: No StorableItem with ID '{_requiredItemId}' founded to put on '{_slotId}'.");
        }
    }
}
