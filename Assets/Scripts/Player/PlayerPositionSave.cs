using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    public class PlayerPositionSave : MonoBehaviour
    {
        public int _saveOrder = 0;
        public bool _savingDisabled = false;
        private const string _playerTag = "Player";
        private void OnTriggerEnter(Collider collision)
        {
            if (!collision.CompareTag(_playerTag)) return;
            if (_savingDisabled) return;
            PlayerPrefs.SetInt("POS_ID", _saveOrder);
            PlayerPrefs.Save();
            Debug.Log("Position saved: " + _saveOrder);
        }

        public static void DisableSavingForAll()
        {
            PlayerPositionSave[] allSaves = FindObjectsOfType<PlayerPositionSave>();
            foreach (var save in allSaves)
            {
                save._savingDisabled = true;
            }
        }

        public void EnableSaving()
        {
            _savingDisabled = false;
        }

        public void DisableSaving()
        {
            _savingDisabled = true;
        }
    }
}
