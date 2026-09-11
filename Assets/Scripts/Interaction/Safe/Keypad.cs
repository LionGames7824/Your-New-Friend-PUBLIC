using UnityEngine;

namespace YourNewFriend
{
    public class Keypad : MonoBehaviour
    {
        [Tooltip("Whatever this keypad controls. Must implement IKeypadReceiver (e.g. SafeController).")]
        [SerializeField] private MonoBehaviour _receiverBehaviour;

        private void Awake()
        {
            IKeypadReceiver receiver = _receiverBehaviour as IKeypadReceiver;

            if (receiver == null)
            {
                Debug.LogError($"{name}: the assigned receiver does not implement IKeypadReceiver.", this);
                return;
            }

            foreach (var button in GetComponentsInChildren<KeypadButton>(true))
            {
                button.SetReceiver(receiver);
            }
        }
    }
}
