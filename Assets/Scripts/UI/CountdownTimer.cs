using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using YourNewFriend;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private float totalTime;
    private float remainingTime;
    private bool startt = false;


    void Start()
    {
        remainingTime = totalTime;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (startt)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(remainingTime / 60);
                int seconds = Mathf.FloorToInt(remainingTime % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                gameObject.SetActive(false);
                MusicTriggerZone.mus_overlap = false;
                GameStateManager.Instance.OpenGameOver(true);
                remainingTime = totalTime;
                startt = false;
            }
        }
        
    }

    public void StartTimer()
    {
        gameObject.SetActive(true);
        startt = true;
    }

    public void StopTimer()
    {
        gameObject.SetActive(false);
        MusicTriggerZone.mus_overlap = false;
        remainingTime = totalTime;
        startt = false;
    }
}
