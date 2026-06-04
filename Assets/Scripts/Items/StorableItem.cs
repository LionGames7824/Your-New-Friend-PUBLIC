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
        
        public string GetId() => _id;
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