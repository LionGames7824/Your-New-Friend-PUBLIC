using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YourNewFriend;

public class CountdownTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _onCountdownStart;
    public CountdownTimer timer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCountdown();
        }
    }

    public void StartCountdown()
    {
        timer.StartTimer();
        MusicTriggerZone.mus_overlap = true;
        _onCountdownStart?.Invoke();
    }
}
