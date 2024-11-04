using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YourNewFriend
{
    public class Player_Interactor : MonoBehaviour
    {
        [SerializeField] private Transform _interactionPoint;
        [SerializeField] private float _interactionPointRadius = 0.5f;
        [SerializeField] private LayerMask _interactableMask;

        [SerializeField] private int _numFound;
        private readonly Collider[] _colliders = new Collider[3];

        void Update()
        {
            _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

            if (_numFound > 0)
            {
                var interactable = _colliders[0].GetComponent<IInteractable>();
                Debug.Log(interactable.InteractionPrompt);

                //if (interactable != null && Input.GetKey("i".ToLower()))
                //{
                //    interactable.Interact(this);
                //}
            }            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_interactionPoint.position, _interactionPointRadius);
        }
    }
}