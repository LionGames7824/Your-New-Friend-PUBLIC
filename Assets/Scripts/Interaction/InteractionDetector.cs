using System.Collections.Generic;
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

        // Sorted nearest-to-farthest by distance below. Cached once instead
        // of building a new comparer every frame.
        private static readonly IComparer<RaycastHit> DistanceComparer =
            Comparer<RaycastHit>.Create((a, b) => a.distance.CompareTo(b.distance));

        // Reused every frame with Physics.RaycastNonAlloc so scanning for a
        // target doesn't allocate a new array on every Update().
        private readonly RaycastHit[] _hitBuffer = new RaycastHit[8];

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

            int hitCount = Physics.RaycastNonAlloc(origin, direction, _hitBuffer, _raycastDistance, _raycastLayers);
            if (hitCount > 0)
            {
                System.Array.Sort(_hitBuffer, 0, hitCount, DistanceComparer);

                // Walk hits nearest-to-farthest. The first interactable we
                // find wins, even if it sits on the same layer used for
                // obstruction (e.g. a keypad mounted flush on a wall panel) -
                // what matters is that nothing interactable is *closer* to
                // the player than it. If we reach an obstruction hit before
                // any interactable, stop: everything past that point
                // (including whatever the obstruction is hiding) is
                // unreachable, which is what keeps players from interacting
                // with things through walls or from reaching inside a
                // locked container before it's actually open.
                for (int i = 0; i < hitCount; i++)
                {
                    RaycastHit hit = _hitBuffer[i];

                    IInteractable candidate = hit.collider.GetComponent<IInteractable>();
                    if (candidate != null)
                    {
                        target = candidate;
                        hitPoint = hit.point;
                        break;
                    }

                    bool isObstruction = (_raycastObstructionLayer.value & (1 << hit.collider.gameObject.layer)) != 0;
                    if (isObstruction)
                        break;
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
