using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    /// <summary>
    /// Represents an interactable item that can be picked up by the player
    /// and stored in the inventory. This class uses internal data tracking
    /// to determine whether the item has already been collected and ensures
    /// persistent state across game sessions.
    /// </summary>

    public class StorableItem : MonoBehaviour, IInteractable
    {
        [System.Serializable]
        private class Data
        {
            public enum State
            {
                Locked, // Not visible to player
                Unlocked, // Visible to player
                Granted // Player got it already
            }

            public State StorableItemState;
        }

        [SerializeField] private string _id;
        [SerializeField] private int quantity = 1;
        [SerializeField] private string _prompt;

        [Tooltip("If true, this item goes straight into the player's hand the moment it's picked up, instead of just going into the inventory.")]
        [SerializeField] private bool _autoEquipOnPickup = false;

        [Header("Hold In Hand")]
        [Tooltip("Local position offset applied when this item is held in the player's hand.")]
        [SerializeField] private Vector3 _handPositionOffset = new Vector3(0f, 0.05f, 0.1f);
        [Tooltip("Local rotation offset (Euler angles) applied when this item is held in the player's hand.")]
        [SerializeField] private Vector3 _handRotationOffset = new Vector3(0f, 90f, 0f);
        
        public string GetId() => _id;
        public Vector3 HandPositionOffset => _handPositionOffset;
        public Vector3 HandRotationOffset => _handRotationOffset;
        public string InteractionPrompt => _prompt;

        private Data _data;
        private Material _material;

        private void Awake()
        {
            LoadData();
        }

        private void Start()
        {
            UpdateState();
        }

        private void LoadData()
        {
            if (PersistenceDataHelper.TryLoadPersistedData<Data>(_id, out _data))
            {
                Debug.Log($"{GetType().Name}: Data loaded!");
            }
            else
            {
                _data = new()
                {
                    StorableItemState = Data.State.Unlocked
                };
                Debug.Log($"{GetType().Name}: No data file found, creating a new one.");
            }
            
        }

        private void UpdateState()
        {
            switch (_data.StorableItemState)
            {
                case Data.State.Locked:
                    // TODO: handle locked state
                    // Not visible to player
                    
                    break;
                case Data.State.Unlocked:
                    // TODO: handle Unlocked state
                    // Visible to player

                    break;
                case Data.State.Granted:
                    gameObject.SetActive(false);
                    break;
            }
        }

        private void UpdateData()
        {
            if (_data.StorableItemState == Data.State.Unlocked)
            {
                _data.StorableItemState = Data.State.Granted;
                SaveData();
                InventoryManager.Instance.AddItem(_id, 1);

                if (_autoEquipOnPickup)
                    InventoryManager.Instance.EquipItem(_id);
            }

            UpdateState();
        }

        public void SaveData()
        {
            if (PersistenceDataHelper.TryPersistData(_id, _data))
            {
                Debug.Log($"{GetType().Name}: {_id} saved!");
            }
        }

        public bool Interact(PlayerController interactor)
        {
            UpdateData();
            return true;
        }

        public void Reactivate()
        {
            _data.StorableItemState = Data.State.Unlocked;
            SaveData();
            gameObject.SetActive(true);
        }

    }
}