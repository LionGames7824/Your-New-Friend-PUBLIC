using UnityEngine;

namespace YourNewFriend
{
    public class PanelLightController : MonoBehaviour
    {
        [Header("Materials")]
        [SerializeField] private Material _materialOff;
        [SerializeField] private Material _materialOn;

        private MeshRenderer _renderer;

        private void Awake()
        {            
            _renderer = GetComponent<MeshRenderer>();
            SetLight(false);
        }

        public void SetLight(bool state)
        {
            if (_renderer == null) return;
                        
            _renderer.material = state ? _materialOn : _materialOff;
        }

        public void TurnOn() => SetLight(true);
        public void TurnOff() => SetLight(false);
    }
}