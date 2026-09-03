using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTrigger : MonoBehaviour
{
    public CountdownTimer timer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            timer.StartTimer();
    }
}
