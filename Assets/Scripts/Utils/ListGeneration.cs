using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YourNewFriend;

public class ListGeneration : MonoBehaviour
{
    public static List<GameObject> doors;

    void Awake()
    {
        doors = new List<GameObject>();
    }
}
