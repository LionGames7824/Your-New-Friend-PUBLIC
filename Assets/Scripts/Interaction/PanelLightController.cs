using UnityEngine;

namespace YourNewFriend
{
    public enum PanelLightState { OffGreen, OffRed, Green, Red }

    public class PanelLightController : MonoBehaviour
    {
        [Header("Materials")]
        [SerializeField] private Material _materialOffGreen;
        [SerializeField] private Material _materialOffRed;
        [SerializeField] private Material _materialGreen;
        [SerializeField] private Material _materialRed;

        [Header("Settings")]
        [SerializeField] private PanelLightState _initialState = PanelLightState.OffGreen;

        private MeshRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            SetLight(_initialState);
        }

        public void SetLight(PanelLightState state)
        {
            if (_renderer == null) return;

            _renderer.material = state switch
            {
                PanelLightState.Green => _materialGreen,
                PanelLightState.Red => _materialRed,
                PanelLightState.OffRed => _materialOffRed,
                PanelLightState.OffGreen => _materialOffGreen,
            };
        }

        public void TurnOffRed() => SetLight(PanelLightState.OffRed);
        public void TurnOffGreen() => SetLight(PanelLightState.OffGreen);
        public void TurnGreen() => SetLight(PanelLightState.Green);
        public void TurnRed() => SetLight(PanelLightState.Red);
    }
}