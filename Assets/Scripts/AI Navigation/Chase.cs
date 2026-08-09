using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Chase : MonoBehaviour
{
    private int destPoint = 0;
    private bool permanence = false;
    private NavMeshAgent agent;
    public static bool isPlayerSeen = false;
    public Transform player;
    public UnityEvent death;
    public Transform sBot;
    public float searchTime = 10f;
    public float catchSpeed = 9f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
    }

    
    void Update()
    {
        if (isPlayerSeen && !permanence)
        {
            agent.destination = player.position;
            if (Physics.Raycast(sBot.position, player.position - sBot.position, out RaycastHit hit) && sBot.InverseTransformVector(hit.point - sBot.position).z > 0)
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
        else if (!isPlayerSeen && !permanence)
        {
            if (Physics.Raycast(sBot.position, player.position - sBot.position, out RaycastHit hit) && sBot.InverseTransformVector(hit.point - sBot.position).z > 0)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    isPlayerSeen = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            TriggerDeath();
    }

    private IEnumerator LoseInterest()
    {
        yield return new WaitForSeconds(searchTime);
        isPlayerSeen = false;
    }

    public void TriggerDeath()
    {
        death.Invoke();
        Debug.Log("Player has been caught!");
    }

    public void InitiateChase()
    {
        agent.speed = catchSpeed;
        agent.destination = player.position;
        permanence = true;
    }

    public void StopChase()
    {
        isPlayerSeen = false;
        permanence = false;
        agent.ResetPath();
    }
}
