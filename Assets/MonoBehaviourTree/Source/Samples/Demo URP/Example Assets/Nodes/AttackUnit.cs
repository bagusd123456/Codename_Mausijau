﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [MBTNode("Example/Attack Unit")]
    [AddComponentMenu("")]
    public class AttackUnit : Leaf
    {
        public GameObjectReference targetToAttack;
        public TransformReference attackUnit;

        public override NodeResult Execute()
        {
            GameObject targetGO = targetToAttack.Value;
            Transform attackUnitGO = attackUnit.Value;
            // Move as long as distance is greater than min. distance
            if (targetGO != null)
            {
                if (targetGO.TryGetComponent(out UnitCondition targetEnemy))
                {
                    if (!attackUnitGO.TryGetComponent(out UnitCondition attackingUnit))
                    {
                        return NodeResult.failure;
                    }

                    if (targetEnemy.isDead)
                    {
                        return NodeResult.failure;
                    }

                    if (attackingUnit.isDead)
                    {
                        return NodeResult.failure;
                    }

                    attackingUnit.GetComponent<CharacterAttack>().Attack(targetEnemy.transform);

                    //targetEnemy.TakeDamage(damage);
                    //Debug.Log($"<b>{targetEnemy.name}</b> taken <color=red>{damage} damage...</color>");
                    return NodeResult.success;
                }

                return NodeResult.running;
            }
            else
            {
                return NodeResult.failure;
            }
        }
    }
}
