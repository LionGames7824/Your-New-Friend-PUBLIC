using TMPro;
using UnityEngine;

namespace YourNewFriend
{
    public class SafeDisplayUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _displayText;
        [SerializeField] private SafeManager _safeManager;

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
            _displayText.text = new string('*', currentInput.Length);
        }
    }
}