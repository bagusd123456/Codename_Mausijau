using System.Collections;
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
        public int damage = 1;

        public override NodeResult Execute()
        {
            GameObject targetGO = targetToAttack.Value;
            // Move as long as distance is greater than min. distance
            if (targetGO != null)
            {
                if (targetGO.TryGetComponent(out UnitCondition targetEnemy))
                {
                    if (targetEnemy.isDead)
                    {
                        return NodeResult.failure;
                    }

                    targetEnemy.TakeDamage(damage);
                    Debug.Log($"<b>{targetEnemy.name}</b> taken <color=red>{damage} damage...</color>");
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
