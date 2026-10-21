
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YourNewFriend
{
    public class ButtonTextColor : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _buttonText;

        [Header("Text Colors")]
        [SerializeField]
        private Color _normalColor =
    new Color32(204, 204, 204, 255);

        [SerializeField]
        private Color _disabledColor =
            new Color32(115, 115, 115, 255);

        private void Awake()
        {
            if (_button == null)
                _button = GetComponent<Button>();

            if (_buttonText == null)
                _buttonText = GetComponentInChildren<TMP_Text>(true);
        }

        private void Update()
        {
            if (_button == null || _buttonText == null)
                return;

            _buttonText.color = _button.interactable
                ? _normalColor
                : _disabledColor;
        }
    }
}