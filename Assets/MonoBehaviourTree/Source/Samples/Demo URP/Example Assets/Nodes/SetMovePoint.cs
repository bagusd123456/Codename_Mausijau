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

            // Stop after last waypoint
            if (index >= waypoints.Length)
            {
                return NodeResult.failure;
            }
            else if (index == waypoints.Length - 1)
            {
                return NodeResult.success;
            }
            else
            {
                if (index < waypoints.Length)
                    index += 1;
                // Set blackboard variable with need waypoint (position)
                variableToSet.Value = waypoints[index];
                return NodeResult.success;
            }
        }
    }
}
