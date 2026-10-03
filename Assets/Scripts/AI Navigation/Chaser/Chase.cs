using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using YourNewFriend;

namespace YourNewFriend
{
    public class Chase : MonoBehaviour
    {
        private int destPoint = 0;
        private bool permanence = false;
        private NavMeshAgent agent;
        private Vector3 startPosition;
        private Coroutine loseInterestRoutine;
        public static bool isPlayerSeen = false;
        public Transform capsule;
        public Transform player;
        public Transform sBot;
        public float searchTime = 10f;
        public float catchSpeed = 7f;

        private bool mus_flag1 = false;
        private bool mus_flag2 = false;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            startPosition = transform.position;
        }


        void Update()
        {
            if (isPlayerSeen && !permanence)
            {
                mus_flag2 = false;
                if (mus_flag1 == false)
                {
                    if (MusicTriggerZone.mus_overlap == false)
                    {
                        MusicTriggerZone.mus_overlap = true;
                        AudioManager.Instance.PlayMusicWithoutSaving("WHEREISHE", true);
                    }
                    mus_flag1 = true;
                }
                agent.destination = player.position;
                if (Physics.Raycast(sBot.position, capsule.position - sBot.position, out RaycastHit hit) && sBot.InverseTransformVector(hit.point - sBot.position).z > 0)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        if (loseInterestRoutine != null) StopCoroutine(loseInterestRoutine);
                        loseInterestRoutine = StartCoroutine(LoseInterest());
                    }
                    else if (loseInterestRoutine == null)
                    {
                        loseInterestRoutine = StartCoroutine(LoseInterest());
                    }
                }
            }
            else if (!isPlayerSeen && !permanence)
            {
                mus_flag1 = false;
                if (mus_flag2 == false)
                {
                    MusicTriggerZone.mus_overlap = false;
                    AudioManager.Instance.PlayBuffer();
                    mus_flag2 = true;
                }
                if (Physics.Raycast(sBot.position, capsule.position - sBot.position, out RaycastHit hit) && sBot.InverseTransformVector(hit.point - sBot.position).z > 0)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("Player is seen");
                        isPlayerSeen = true;
                    }
                }
            }
            else
            {
                agent.destination = player.position;
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
            loseInterestRoutine = null;
        }

        public void TriggerDeath()
        {
            GameStateManager.Instance.OpenGameOver(true);
            Debug.Log("Player has been caught!");
        }

        public void InitiateChase()
        {
            agent.speed = catchSpeed;
            isPlayerSeen = true;
            agent.destination = player.position;
            permanence = true;
            MusicTriggerZone.mus_overlap = true;
            AudioManager.Instance.PlayMusicWithoutSaving("WHEREISHE", true);
            Debug.Log(agent.destination);
        }

        public void StopChase()
        {
            if (loseInterestRoutine != null)
            {
                StopCoroutine(loseInterestRoutine);
                loseInterestRoutine = null;
            }

            isPlayerSeen = false;
            permanence = false;
            agent.ResetPath();
            MusicTriggerZone.mus_overlap = false;
            AudioManager.Instance.PlayBuffer();
        }

        /// <summary>
        /// Connect this to the GameOverUI's onResume event, so the bot gives up the chase
        /// and warps back to where it started as soon as the player respawns.
        /// </summary>
        public void ReturnToStartPosition()
        {
            StopChase();
            agent.Warp(startPosition);
        }
    }
}