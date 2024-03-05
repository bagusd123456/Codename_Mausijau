using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [MBTNode("Example/Set Move Point From Manager")]
    [AddComponentMenu("")]
    public class SetMovePointFromManager : Leaf
    {
        public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);
        private List<Transform> waypoints;
        public CharacterMovement characterMovement;
        public int index = -1;
        private int direction = 1;

        public override NodeResult Execute()
        {
            waypoints = characterMovement.waypointsList;
            if (waypoints.Count == 0)
            {
                return NodeResult.failure;
            }

            // Stop after last waypoint
            if (index > waypoints.Count)
            {
                return NodeResult.failure;
            }
            else if (index == waypoints.Count - 1)
            {
                return NodeResult.success;
            }
            else
            {
                if (index < waypoints.Count)
                    index += 1;
                // Set blackboard variable with need waypoint (position)
                variableToSet.Value = waypoints[index];
                return NodeResult.success;
            }
        }
    }
}
