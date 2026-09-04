using TMPro;
using UnityEngine;

namespace YourNewFriend
{
    public class SafeDisplayUI : MonoBehaviour
    {
        [SerializeField] private SafeManager _safeManager;
        [SerializeField] private GameObject[] _asteriskSlots; 

        private void OnEnable()
        {
            _safeManager.OnInputChanged += HandleInputChanged;
        }

        private void OnDisable()
        {
            _safeManager.OnInputChanged -= HandleInputChanged;
        }

        private void HandleInputChanged(string currentInput)
        {
            for (int i = 0; i < _asteriskSlots.Length; i++)
            {
                _asteriskSlots[i].SetActive(i < currentInput.Length);
            }
        }
    }
}