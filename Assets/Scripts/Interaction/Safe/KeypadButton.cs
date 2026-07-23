using UnityEngine;

namespace YourNewFriend
{
    public enum KeypadButtonType { Digit, Clear, Confirm }

    public class KeypadButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private KeypadButtonType _buttonType;
        [SerializeField] private string _digitValue; // só usado se Digit, ex: "7"
        [SerializeField] private string _interactionPrompt = "Press";

        public string InteractionPrompt => _interactionPrompt;

        public bool Interact(PlayerController interactor)
        {
            Debug.Log($"Pressed ${_digitValue}");

            SafeManager.Instance.HandleInput(_buttonType, _digitValue);
            return true;
        }
    }
}