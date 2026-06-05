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
            // Pega o Renderer da própria esfera onde o script está
            _renderer = GetComponent<MeshRenderer>();

            // Garante que comece com a luz desligada
            SetLight(false);
        }

        // Esta é a função que você vai chamar pelo UnityEvent
        public void SetLight(bool state)
        {
            if (_renderer == null) return;

            // Troca o material dependendo do estado
            _renderer.material = state ? _materialOn : _materialOff;
        }

        // Função auxiliar para o UnityEvent (caso ele não aceite bool direto)
        public void TurnOn() => SetLight(true);
        public void TurnOff() => SetLight(false);
    }
}