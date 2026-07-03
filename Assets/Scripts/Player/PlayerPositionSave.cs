using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    public class PlayerPositionSave : MonoBehaviour
    {
        public int _saveOrder = 0;
        private const string _playerTag = "Player";
        private void OnTriggerEnter(Collider collision)
        {
            if (!collision.CompareTag(_playerTag)) return;
            PlayerPrefs.SetInt("POS_ID", _saveOrder);
            PlayerPrefs.Save();
            Debug.Log("Position saved: " + _saveOrder);
        }
    }
}
