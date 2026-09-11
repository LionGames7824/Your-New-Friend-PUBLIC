using System;

namespace YourNewFriend
{
    public interface IKeypadReceiver
    {
        event Action<string> OnInputChanged;

        void HandleInput(KeypadButtonType type, string digit);
    }
}
