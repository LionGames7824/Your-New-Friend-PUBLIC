using System.Collections;
using UnityEngine;

namespace YourNewFriend
{
    public class TaserWireEffect : MonoBehaviour
    {
        [SerializeField] private LineRenderer _wire;

        [Header("Timing")]
        [Tooltip("How long the wire stays visible after firing.")]
        [SerializeField] private float _duration = 0.15f;
        [Tooltip("How long the wire takes to grow from 0 to full width.")]
        [SerializeField] private float _growInTime = 0.03f;

        [Header("Visuals")]
        [SerializeField] private float _width = 0.02f;
        [Tooltip("How many points the wire is built from. More points = smoother jitter/curve.")]
        [SerializeField] private int _segmentCount = 8;
        [Tooltip("How far each point can wobble sideways off the straight line, for a crackling energy look.")]
        [SerializeField] private float _jitterAmount = 0.05f;

        private Coroutine _activeShot;

        private void Awake()
        {
            if (_wire != null)
                _wire.enabled = false;
        }

        public void Fire(Vector3 origin, Vector3 endPoint)
        {
            if (_wire == null)
                return;

            if (_activeShot != null)
                StopCoroutine(_activeShot);

            _activeShot = StartCoroutine(PlayShot(origin, endPoint));
        }

        private IEnumerator PlayShot(Vector3 origin, Vector3 endPoint)
        {
            _wire.enabled = true;
            float elapsed = 0f;

            while (elapsed < _duration)
            {
                UpdateWireVisuals(origin, endPoint, elapsed);
                elapsed += Time.deltaTime;
                yield return null;
            }

            _wire.enabled = false;
            _activeShot = null;
        }

        private void UpdateWireVisuals(Vector3 origin, Vector3 endPoint, float elapsed)
        {
            float growIn = _growInTime > 0f ? Mathf.Clamp01(elapsed / _growInTime) : 1f;
            _wire.widthMultiplier = _width * growIn;

            int segmentCount = Mathf.Max(_segmentCount, 2);
            _wire.positionCount = segmentCount;

            Vector3 direction = (endPoint - origin).normalized;
            Vector3 sideA = Vector3.Cross(direction, Vector3.up);
            if (sideA.sqrMagnitude < 0.0001f)
                sideA = Vector3.Cross(direction, Vector3.right);
            sideA.Normalize();
            Vector3 sideB = Vector3.Cross(direction, sideA);

            for (int i = 0; i < segmentCount; i++)
            {
                float t = i / (float)(segmentCount - 1);
                Vector3 point = Vector3.Lerp(origin, endPoint, t);

                float edgeFade = Mathf.Sin(t * Mathf.PI);
                Vector3 jitter =
                    (sideA * Random.Range(-1f, 1f) +
                     sideB * Random.Range(-1f, 1f)) * _jitterAmount * edgeFade;

                _wire.SetPosition(i, point + jitter);
            }
        }
    }
}
