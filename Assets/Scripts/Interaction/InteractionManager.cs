using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        private string _currentItemId;
        public string CurrentItemId => _currentItemId;
        public bool HasItemEquipped => !string.IsNullOrEmpty(_currentItemId);

        public void EquipItem (string _itemid)
        {
            _currentItemId = _itemid;
        }
    }
}