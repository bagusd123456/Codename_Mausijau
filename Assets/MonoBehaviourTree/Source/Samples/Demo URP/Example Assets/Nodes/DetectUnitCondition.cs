using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Detect Unit Condition Service")]
    public class DetectUnitConditionService : Service
    {
        public ObjectSelector objectSelector;
        
        public Transform target;
        public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);
        public BoolReference onCommandToSet = new BoolReference(VarRefMode.DisableConstant);
        public TransformReference targetReset = new TransformReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            if (objectSelector.selectedTransform != null)
            {
                //target = objectSelector.currentSelectedArmy;
            }

            if (target != null)
            {

                variableToSet.Value = target;
                onCommandToSet.Value = true;

                //targetReset.Value = null;
            }
            else
            {
                variableToSet.Value = null;
                onCommandToSet.Value = false;
            }
        }
    }
}

