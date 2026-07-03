using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    private int destPoint = 0;
    private NavMeshAgent agent;
    public static bool isPlayerSeen = false;
    public Transform player;
    public Transform sBot;
    public float searchTime = 10f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    
    void Update()
    {
        if (isPlayerSeen)
        {
            agent.destination = player.position;
            if (Physics.Raycast(sBot.position, player.position - sBot.position, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    StopCoroutine(LoseInterest());
                    StartCoroutine(LoseInterest());
                }
                else
                {
                    StartCoroutine(LoseInterest());
                }
            }
        }
        else
        {
            if (Physics.Raycast(sBot.position, player.position - sBot.position, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    isPlayerSeen = true;
                }
            }
        }
    }

    private IEnumerator LoseInterest()
    {
        yield return new WaitForSeconds(searchTime);
        isPlayerSeen = false;
    }
}
