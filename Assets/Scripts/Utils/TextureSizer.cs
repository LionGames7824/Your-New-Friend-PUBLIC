using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace YourNewFriend
{
    [ExecuteInEditMode]
    public class TextureSizer : MonoBehaviour
    {
        // Start is called before the first frame update
        public float x_size = 1.0f;
        public float y_size = 1.0f;
        public float z_size = 1.0f;
        public bool x = true;
        public bool y = true;
        public bool z = false;
        Material material;
        void Start()
        {
            material = GetComponent<Renderer>().material;
            Material new_material = new Material(material);
            GetComponent<Renderer>().material = new_material;
            Vector3 iscale = transform.localScale;
            SetTextureTiling(new_material, iscale);
        }

        // Update is called once per frame
        void Update()
        {
            SetTextureTiling(GetComponent<Renderer>().material, transform.localScale);
        }

        void SetTextureTiling(Material material, Vector3 scale)
        {
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