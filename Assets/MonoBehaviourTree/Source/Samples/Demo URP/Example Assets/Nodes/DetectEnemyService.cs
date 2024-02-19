using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Detect Enemy Service")]
    public class DetectEnemyService : Service
    {
        private bool listenerInitialized;
        private bool foundListener;
        public LayerMask mask = -1;
        [Tooltip("Sphere radius")]
        public float range = 15;
        public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);
        public GameObjectReference enemyGameObject = new GameObjectReference(VarRefMode.DisableConstant);
        public TransformReference selfCondition = new TransformReference(VarRefMode.DisableConstant);
        public BoolReference onCommand = new BoolReference();
        public TransformReference commandTargetReference = new TransformReference(VarRefMode.DisableConstant);

        public override void Task()
        {
            if (onCommand.Value) return;
            // Find target in radius and feed blackboard variable with results
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, mask, QueryTriggerInteraction.Ignore);
            if (colliders.Length > 0)
            {
                variableToSet.Value = colliders[0].transform;
                enemyGameObject.Value = colliders[0].gameObject;
            }
            else
            {
                if (foundListener)
                {
                    if (enemyGameObject.Value.GetComponent<UnitCondition>().isDead)
                    {
                        variableToSet.Value = null;
                        enemyGameObject.Value = null;
                        foundListener = false;
                    }
                    return;
                }

                variableToSet.Value = null;
                enemyGameObject.Value = null;
            }
        }

        public void OnAllyAttacked(UnitCondition attackerUnit)
        {
            if (!attackerUnit.isDead && enemyGameObject.Value == null)
            {
                foundListener = true;
                variableToSet.Value = attackerUnit.transform;
                enemyGameObject.Value = attackerUnit.gameObject;
            }
        }

        private void OnEnable()
        {
            var alliedUnit = selfCondition.Value.GetComponent<UnitCondition>().unitArmy;
            if (alliedUnit != null)
            {
                //Debug.LogWarning($"Condition Registered...");
                foreach (var allyCondition in alliedUnit)
                {
                    allyCondition.OnUnitAttacked += attackerUnit =>
                    {
                        OnAllyAttacked(attackerUnit);
                    };
                }
            }

            if (selfCondition.Value.TryGetComponent(out UnitCondition unit))
            {
                unit.OnUnitAttacked += attackerUnit =>
                {
                    onCommand.Value = false;
                    commandTargetReference.Value = null;

                    OnAllyAttacked(attackerUnit);
                };
            }
        }

        private void OnDisable()
        {
            foundListener = false;

            var alliedUnit = selfCondition.Value.GetComponent<UnitCondition>().unitArmy;
            if (alliedUnit != null)
            {
                //Debug.LogWarning($"Condition Not Registered...");
                foreach (var allyCondition in alliedUnit)
                {
                    allyCondition.OnUnitAttacked -= attackerUnit =>
                    {
                        OnAllyAttacked(attackerUnit);
                    };
                }
            }

            if (selfCondition.Value.TryGetComponent(out UnitCondition unit))
            {
                unit.OnUnitAttacked -= attackerUnit =>
                {
                    OnAllyAttacked(attackerUnit);
                };
            }
        }
    }
}
