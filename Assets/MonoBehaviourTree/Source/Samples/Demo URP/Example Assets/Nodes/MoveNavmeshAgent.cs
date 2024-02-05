﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Move Navmesh Agent")]
    public class MoveNavmeshAgent : Leaf
    {
        public TransformReference destination;
        public bool useVector3;
        public Vector3Reference destinationVector3;
        public NavMeshAgent agent;
        public float stopDistance = 2f;
        [Tooltip("How often target position should be updated")]
        public float updateInterval = 1f;
        private float time = 0;

        public override void OnEnter()
        {
            time = 0;
            agent.isStopped = false;
            if (useVector3)
                agent.SetDestination(destinationVector3.Value);
            else
                agent.SetDestination(destination.Value.position);
        }
        
        public override NodeResult Execute()
        {
            time += Time.deltaTime;
            // Update destination every given interval
            if (time > updateInterval)
            {
                // Reset time and update destination
                time = 0;
                if(useVector3)
                    agent.SetDestination(destinationVector3.Value);
                else
                    agent.SetDestination(destination.Value.position);
            }
            // Check if path is ready
            if (agent.pathPending)
            {
                return NodeResult.running;
            }
            // Check if agent is very close to destination
            if (agent.remainingDistance < stopDistance)
            {
                return NodeResult.success;
            }
            // Check if there is any path (if not pending, it should be set)
            if (agent.hasPath)
            {
                return NodeResult.running;
            }

            agent.isStopped = true;
            // By default return failure
            return NodeResult.failure;
        }

        public override void OnExit()
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            // agent.ResetPath();
        }

        public override bool IsValid()
        {
            return !(destination.isInvalid && !useVector3 || agent == null);
        }
    }
}