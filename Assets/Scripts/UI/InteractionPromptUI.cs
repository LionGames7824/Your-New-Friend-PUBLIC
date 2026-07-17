using TMPro;
using UnityEngine;

namespace YourNewFriend
{
    public class InteractionPromptUI : Singleton<InteractionPromptUI>
    {
        [SerializeField] private RectTransform _promptRoot;
        [SerializeField] private TMP_Text _promptText;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private InteractionDetector _interactionDetector;

        protected override void Awake()
        {
            base.Awake();
            _promptRoot.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _interactionDetector.OnTargetChanged += HandleTargetChanged;
        }

        private void OnDisable()
        {
            _interactionDetector.OnTargetChanged -= HandleTargetChanged;
        }

        private void HandleTargetChanged(IInteractable interactable, Vector3 hitPoint)
        {
            if (interactable == null || string.IsNullOrEmpty(interactable.InteractionPrompt))
            {
                _promptRoot.gameObject.SetActive(false);
                return;
            }            

            _promptText.text = interactable.InteractionPrompt;

            Vector2 screenPoint = _mainCamera.WorldToScreenPoint(hitPoint);

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                (RectTransform)_promptRoot.parent,
                screenPoint,
                _mainCamera,
                out Vector3 worldPoint);

            _promptRoot.position = worldPoint;
            _promptRoot.gameObject.SetActive(true);
        }
    }
}