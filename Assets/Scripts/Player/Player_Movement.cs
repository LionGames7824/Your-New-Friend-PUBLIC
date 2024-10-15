using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YourNewFriend{
    public class Player_Movement : MonoBehaviour
    {
        public GameObject Player;
        public int Speed;
        void Start()
        {

        }

        private void FixedUpdate()
        {
            if (Input.GetKey("w".ToLower()))
            {
                Player.transform.position += Player.transform.forward * Speed * Time.deltaTime;
            }
            if (Input.GetKey("a".ToLower()))
            {
                Player.transform.position += Player.transform.right * Speed * -1 * Time.deltaTime;
            }
            if (Input.GetKey("s".ToLower()))
            {
                Player.transform.position += Player.transform.forward * Speed * -1 * Time.deltaTime;
            }
            if (Input.GetKey("d".ToLower()))
            {
                Player.transform.position += Player.transform.right * Speed * Time.deltaTime;
            }
        }
    }
}