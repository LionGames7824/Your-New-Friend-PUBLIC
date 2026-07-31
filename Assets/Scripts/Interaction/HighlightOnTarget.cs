using UnityEngine;

namespace YourNewFriend
{
    public class HighlightOnTarget : MonoBehaviour, IHighlightable
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _highlightColor = new Color(0.75f, 0.75f, 0.75f); 

        private MaterialPropertyBlock _propBlock;
        private Color _originalColor;
        private static readonly int ColorProp = Shader.PropertyToID("_Color");

        private void Awake()
        {
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();

            _propBlock = new MaterialPropertyBlock();
            _originalColor = _renderer.sharedMaterial.color;
        }

        public void SetHighlighted(bool isHighlighted)
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor(ColorProp, isHighlighted ? _highlightColor : _originalColor);
            _renderer.SetPropertyBlock(_propBlock);
        }
    }
}