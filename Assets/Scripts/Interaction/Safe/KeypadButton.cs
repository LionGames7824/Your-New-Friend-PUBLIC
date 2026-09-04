using UnityEngine;

namespace YourNewFriend
{
    public enum KeypadButtonType { Digit, Clear, Confirm }

    public class KeypadButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private KeypadButtonType _buttonType;
        [SerializeField] private string _digitValue;
        [SerializeField] private string _interactionPrompt = "Press";

        public string InteractionPrompt => _interactionPrompt;

        public bool Interact(PlayerController interactor)
        {
            SafeManager.Instance.HandleInput(_buttonType, _digitValue);
            return true;
        }
    }
}