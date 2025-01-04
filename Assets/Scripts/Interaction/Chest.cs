using UnityEngine;

namespace YourNewFriend
{
    public class Chest : MonoBehaviour, IInteractable
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
        [SerializeField] private Color _unlockedColor;

        private const string _colorMaterial = "_Color";

        public string InteractionPrompt => _prompt;

        private Data _data;
        private Material _material;

        private void Awake()
        {
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            _material = meshRenderer.material;

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
                    break;
                case Data.State.Unlocked:
                    _material.SetColor(_colorMaterial, _unlockedColor);
                    break;
                case Data.State.Granted:
                    _material.SetColor(_colorMaterial, Color.gray);
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
                Debug.Log("Chest saved!");
            }
        }

        public bool Interact(PlayerController interactor)
        {
            UpdateData();
            return true;
        }
    }
}