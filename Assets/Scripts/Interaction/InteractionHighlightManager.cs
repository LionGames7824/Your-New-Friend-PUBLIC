using UnityEngine;

namespace YourNewFriend
{
    public class InteractionHighlightManager : MonoBehaviour
    {
        [SerializeField] private InteractionDetector _interactionDetector;

        private IHighlightable _currentHighlighted;

        private void OnEnable()
        {
            _interactionDetector.OnTargetChanged += HandleTargetChanged;
        }

        private void OnDisable()
        {
            _interactionDetector.OnTargetChanged -= HandleTargetChanged;
        }

        private void HandleTargetChanged(IInteractable target, Vector3 hitPoint)
        {
            _currentHighlighted?.SetHighlighted(false);

            _currentHighlighted = (target as Component)?.GetComponent<IHighlightable>();
            _currentHighlighted?.SetHighlighted(true);
        }
    }
}