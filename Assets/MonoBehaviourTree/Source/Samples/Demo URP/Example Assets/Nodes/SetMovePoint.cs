using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [MBTNode("Example/Set Move Point")]
    [AddComponentMenu("")]
    public class SetMovePoint : Leaf
    {
        public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);
        public Transform[] waypoints;
        public int index = -1;
        private int direction = 1;

        public override NodeResult Execute()
        {
            if (waypoints.Length == 0)
            {
                return NodeResult.failure;
            }

            index += 1;

            // Stop after last waypoint
            if (index >= waypoints.Length)
            {
                return NodeResult.failure;
            }
            else
            {
                // Set blackboard variable with need waypoint (position)
                variableToSet.Value = waypoints[index];
                return NodeResult.success;
            }
        }
    }
}
