using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace YourNewFriend
{
    [ExecuteInEditMode]
    public class TextureSizer : MonoBehaviour
    {
        public float x_size = 1.0f;
        public float y_size = 1.0f;
        public float z_size = 1.0f;
        public bool x = true;
        public bool y = true;
        public bool z = false;

        private Material _material;

        void Start()
        {
            var renderer = GetComponent<Renderer>();

            // Only clone material in play mode
            if (Application.isPlaying)
            {
                _material = renderer.material; 
                Material newMaterial = new Material(_material);
                renderer.material = newMaterial;
                _material = newMaterial;
            }
#if UNITY_EDITOR
            else
            {
                if (renderer.sharedMaterial != null)
                {
                    string path = AssetDatabase.GetAssetPath(renderer.sharedMaterial);
                    if (string.IsNullOrEmpty(path)) return;

                }
                _material = new Material(renderer.sharedMaterial);
                _material.name = renderer.sharedMaterial.name + " (Instance)";
                renderer.sharedMaterial = _material;
                EditorUtility.SetDirty(renderer);
            }
#endif 
            SetTextureTiling(_material, transform.localScale);
        }

        void Update()
        {
            if (_material == null)
            {
                var renderer = GetComponent<Renderer>();
                _material = Application.isPlaying ? renderer.material : renderer.sharedMaterial;
            }

            SetTextureTiling(_material, transform.localScale);
        }

        void SetTextureTiling(Material material, Vector3 scale)
        {
            if (material == null) return;

            Vector2 tiling = new Vector2(x_size * scale.x, z_size * scale.z);

            if (x && y)
            {
                tiling = new Vector2(x_size * scale.x, y_size * scale.y);
            }
            else if (y && z)
            {
                tiling = new Vector2(z_size * scale.z, y_size * scale.y);
            }

            material.mainTextureScale = tiling;
        }
    }
}
