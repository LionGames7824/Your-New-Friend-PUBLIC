using System;
using UnityEngine;
using UnityEngine.Events;

namespace YourNewFriend
{
    public class SafeManager : Singleton<SafeManager>
    {
        [SerializeField] private string _correctCode = "1234";
        [SerializeField] private int _maxLength = 4;

        [Header("Events")]
        [SerializeField] private UnityEvent _onUnlocked;
        [SerializeField] private UnityEvent _onWrongCode;

        public event Action<string> OnInputChanged;

        private string _currentInput = "";

        public void HandleInput(KeypadButtonType type, string digit)
        {
            switch (type)
            {
                case KeypadButtonType.Digit:
                    if (_currentInput.Length < _maxLength)
                        _currentInput += digit;
                    break;

                case KeypadButtonType.Clear:
                    _currentInput = "";
                    break;

                case KeypadButtonType.Confirm:
                    CheckCode();
                    break;
            }

            OnInputChanged?.Invoke(_currentInput);
        }

        private void CheckCode()
        {
            if (_currentInput == _correctCode)
            {
                Debug.Log("Acertou mizeravi");
                _onUnlocked.Invoke();
            }
            else
            {                
                _onWrongCode.Invoke();
            }

            _currentInput = "";
            OnInputChanged?.Invoke(_currentInput);
        }
    }
}