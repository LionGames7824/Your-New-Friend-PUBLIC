using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public int Speed;
    void Start()
    {
        
    }

    // Update is called once per frame
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
