using UnityEngine;

namespace YourNewFriend
{
    public class HighlightOnTarget : MonoBehaviour, IHighlightable
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _highlightColor = Color.yellow;

        private MaterialPropertyBlock _propBlock;
        private Color _originalColor;
        private static readonly int ColorProp = Shader.PropertyToID("_Color");

        private void Awake()
        {
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