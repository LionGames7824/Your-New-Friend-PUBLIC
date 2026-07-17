using UnityEngine;

namespace YourNewFriend
{
    public class InteractionDetector : MonoBehaviour
    {
        [SerializeField] private Transform _rayOrigin;
        [SerializeField] private LayerMask _raycastLayers;
        [SerializeField] private LayerMask _raycastObstructionLayer;
        [SerializeField] private float _raycastDistance = 5f;
        [SerializeField] private bool _debugEnabled;

        public event System.Action<IInteractable, Vector3> OnTargetChanged;

        public IInteractable CurrentTarget { get; private set; }

        private void Update()
        {
            Vector3 origin = _rayOrigin.position;
            Vector3 direction = _rayOrigin.forward;

            if (_debugEnabled)
                Debug.DrawLine(origin, origin + direction * _raycastDistance, Color.cyan);

            IInteractable target = null;
            Vector3 hitPoint = default;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, _raycastDistance, _raycastLayers))
            {
                bool obstructed = (_raycastObstructionLayer.value & (1 << hit.collider.gameObject.layer)) != 0;
                if (!obstructed)
                {
                    target = hit.collider.GetComponent<IInteractable>();
                    hitPoint = hit.point;
                }
            }

            if (target != CurrentTarget)
            {
                CurrentTarget = target;
                OnTargetChanged?.Invoke(CurrentTarget, hitPoint);
            }
        }

        public bool TryInteract(PlayerController interactor)
        {
            return CurrentTarget != null && CurrentTarget.Interact(interactor);
        }
    }
}