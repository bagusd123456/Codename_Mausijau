using System;
using System.Collections;
using System.Collections.Generic;
using MBT;
using UnityEngine;
using UnityEngine.Serialization;

namespace MBTExample
{
    [MBTNode("Example/Check Unit Distance")]
    [AddComponentMenu("")]
    public class CheckUnitDistance : Condition
    {
        public TransformReference currentUnit;
        public TransformReference targetUnit;
        public FloatReference distanceToUnit;
        public FloatReference unitAttackRange;

        public override bool Check()
        {
            Transform currentUnitGO = currentUnit.Value;
            Transform targetUnitGO = targetUnit.Value;
            float distance = distanceToUnit.Value;
            // Move as long as distance is greater than min. distance
            if (currentUnitGO != null)
            {
                if (currentUnitGO.TryGetComponent(out UnitCondition currentUnit))
                {
                    if (!targetUnitGO.TryGetComponent(out UnitCondition targetUnit))
                    {
                        return false;
                    }

                    if (targetUnit.isDead)
                    {
                        return false;
                    }

                    if (currentUnit.isDead)
                    {
                        return false;
                    }

                    float range = currentUnit.unitData.baseAttackRange;
                    unitAttackRange.Value = range;
                    //If Distance is closer than the base attack range then return failure
                    if (distance <= range)
                    {
                        return false;
                    }

                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }
    }
}

