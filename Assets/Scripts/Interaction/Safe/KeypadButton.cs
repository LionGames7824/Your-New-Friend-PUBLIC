using UnityEngine;

namespace YourNewFriend
{
    public enum KeypadButtonType { Digit, Clear, Confirm }

    public class KeypadButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private KeypadButtonType _buttonType;
        [SerializeField] private string _digitValue;
        [SerializeField] private string _interactionPrompt = "Press";

        private IKeypadReceiver _receiver;

        public string InteractionPrompt => _interactionPrompt;

        public void SetReceiver(IKeypadReceiver receiver)
        {
            _receiver = receiver;
        }

        public bool Interact(PlayerController interactor)
        {
            if (_receiver == null)
                return false;

            _receiver.HandleInput(_buttonType, _digitValue);
            return true;
        }
    }
}
