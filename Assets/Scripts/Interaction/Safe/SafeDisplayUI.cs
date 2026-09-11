using TMPro;
using UnityEngine;

namespace YourNewFriend
{
    public class SafeDisplayUI : MonoBehaviour
    {
        [Tooltip("Whatever this display is showing the code for. Must implement IKeypadReceiver (e.g. SafeController).")]
        [SerializeField] private MonoBehaviour _receiverBehaviour;
        [SerializeField] private GameObject[] _asteriskSlots;

        private IKeypadReceiver Receiver => _receiverBehaviour as IKeypadReceiver;

        private void OnEnable()
        {
            Receiver.OnInputChanged += HandleInputChanged;
        }

        private void OnDisable()
        {
            Receiver.OnInputChanged -= HandleInputChanged;
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
