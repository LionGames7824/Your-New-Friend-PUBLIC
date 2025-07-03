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

            public State ChestState;
        }

        [SerializeField] private string _id;
        [SerializeField] private string _prompt;

        public string InteractionPrompt => _prompt;

        private Data _data;
        private Material _material;

        private void Awake()
        {
            LoadData();
        }

        private void LoadData()
        {
            if (PersistenceDataHelper.TryLoadPersistedData<Data>(_id, out _data))
            {
                Debug.Log("Data loaded!");
            }
            else
            {
                _data = new()
                {
                    ChestState = Data.State.Unlocked
                };
                Debug.Log("No data file found, creating a new one.");
            }

            UpdateState();
        }

        private void UpdateState()
        {
            switch (_data.ChestState)
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
                    Destroy(gameObject);
                    break;
            }
        }

        private void UpdateData()
        {
            if (_data.ChestState == Data.State.Unlocked)
            {
                _data.ChestState = Data.State.Granted;
                SaveData();
                InventoryManager.Instance.AddItem(_id, 1);
            }

            UpdateState();
        }

        public void SaveData()
        {
            if (PersistenceDataHelper.TryPersistData(_id, _data))
            {
                Debug.Log($"{_id} saved!");
            }
        }

        public bool Interact(PlayerController interactor)
        {
            UpdateData();
            return true;
        }
    }
}