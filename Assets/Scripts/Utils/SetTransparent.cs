using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend
{
    public class SetTransparent : MonoBehaviour
    {
        [SerializeField] private GameObject gameObject;
        void Start()
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}


