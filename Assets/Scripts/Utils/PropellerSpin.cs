using UnityEngine;

namespace YourNewFriend
{
    public class PropellerSpin : MonoBehaviour
    {
        [SerializeField] private Vector3 _axis = Vector3.up;
        [SerializeField] private float _speed = 360f; // degrees per second
        [SerializeField] private bool _reverse = false;

        private void Update()
        {
            float direction = _reverse ? -1f : 1f;
            transform.Rotate(_axis, _speed * direction * Time.deltaTime, Space.Self);
        }
    }
}
