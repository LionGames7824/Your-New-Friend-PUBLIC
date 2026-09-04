using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YourNewFriend;

public class Charging : MonoBehaviour
{
    [SerializeField] private UnityEvent _onCompleted;
    [SerializeField] private GameObject Door;
    [SerializeField] private Animator ChargingStation;

    void Update()
    {
        if (WireSwitch.completed)
        {
            _onCompleted.Invoke();
            Door.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void StartCharging(int seconds)
    {
        StartCoroutine(Charge_Process(seconds));
    }

    IEnumerator Charge_Process(float seconds)
    {
        
        yield return new WaitForSeconds(seconds);
        ChargingStation.Play("CS_Open");
    }
}
