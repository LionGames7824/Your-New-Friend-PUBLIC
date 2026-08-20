using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace YourNewFriend
{
    public class Patrol : MonoBehaviour
    {
        public Transform[] points;
        private int destPoint = 0;
        private NavMeshAgent agent;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            GotoNextPoint();
        }


        public void GotoNextPoint()
        {
            if (points.Length == 0) return;
            agent.destination = points[destPoint].position;
            destPoint++;
            if (destPoint == points.Length) destPoint = 0;
        }


        void Update()
        {
            if (!Chase.isPlayerSeen && !agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
        }
    }
}