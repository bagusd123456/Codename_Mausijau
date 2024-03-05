using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.AI;
using MBT;
using UnityEngine.Serialization;

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
        private float lastDistance;
        [Tooltip("Animation Handler")]
        public CharacterMovement characterMovement;
        public BoolReference commandBoolRef;
        public override void OnEnter()
        {
            time = 0;
            agent.isStopped = false;

            //Bug, sometimes the destination is not set
            if (destination.Value == null && !useVector3)
            {
                //Debug.LogError("Destination is not set. Please set the destination transform or use Vector3");
                return;
            }

            if (useVector3)
                agent.SetDestination(destinationVector3.Value);
            else
                agent.SetDestination(destination.Value.position);

            Vector3 targetLookPos = useVector3 ? destinationVector3.Value : destination.Value.position;
            //characterMovement.LookToTarget(targetLookPos);
        }
        
        public override NodeResult Execute()
        {
            float stuckTime = 0f;
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

                lastDistance = agent.remainingDistance;
                Vector3 targetLookPos = useVector3 ? destinationVector3.Value : destination.Value.position;
                //characterMovement.LookToTarget(targetLookPos);
            }
            // Check if path is ready
            if (agent.pathPending)
            {
                return NodeResult.running;
            }
            // Check if agent is very close to destination
            if (agent.remainingDistance < stopDistance)
            {
                //destination.Value = null;
                commandBoolRef.Value = false;
                return NodeResult.success;
            }
            // Check if agent is stuck for 5 seconds
            if (agent.remainingDistance > lastDistance)
            {
                stuckTime += Time.deltaTime;
                if (stuckTime > 1.5f)
                {
                    //destination.Value = null;
                    commandBoolRef.Value = false;
                    return NodeResult.success;
                }

                return NodeResult.running;
            }
            if (agent.remainingDistance < lastDistance)
            {
                stuckTime = 0;
                lastDistance = agent.remainingDistance;
                return NodeResult.running;
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
            agent.GetComponent<AnimationController>().CurrentState = MovementStates.Idle;
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
