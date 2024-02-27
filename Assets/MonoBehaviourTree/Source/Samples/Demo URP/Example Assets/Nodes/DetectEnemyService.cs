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
        public LayerMask mask = -1;
        [Tooltip("Sphere radius")]
        public float range = 15;
        public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);
        public GameObjectReference enemyGameObject = new GameObjectReference(VarRefMode.DisableConstant);
        public BoolReference onCommand = new BoolReference();

        public TransformReference selfCondition = new TransformReference(VarRefMode.DisableConstant);
        private Action OnUnitAttackedAction;
        private UnitCondition currentUnit;
        public override void Task()
        {
            if (onCommand.Value)
            {
                variableToSet.Value = null;
                enemyGameObject.Value = null;
                return;
            }
            // Find target in radius and feed blackboard variable with results
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, mask, QueryTriggerInteraction.Ignore);
            if (colliders.Length > 0)
            {
                variableToSet.Value = colliders[0].transform;
                enemyGameObject.Value = colliders[0].gameObject;
            }

            if (enemyGameObject.Value == null) return;

            if(enemyGameObject.Value.TryGetComponent(out UnitCondition targetUnit))
            {
                if (targetUnit.isDead)
                {
                    variableToSet.Value = null;
                    enemyGameObject.Value = null;
                }
            }
        }

        private void OnEnable()
        {
            if (selfCondition.Value.TryGetComponent(out UnitCondition selfUnit))
            {
                selfUnit.OnUnitAttacked += OnUnitAttacked;
            }
        }

        private void OnDisable()
        {
            if (selfCondition.Value.TryGetComponent(out UnitCondition selfUnit))
            {
                selfUnit.OnUnitAttacked -= OnUnitAttacked;
            }
        }

        private void OnUnitAttacked(UnitCondition attackingUnit)
        {
            if (enemyGameObject.Value != null && !onCommand.Value) return;
            variableToSet.Value = attackingUnit.transform;
            enemyGameObject.Value = attackingUnit.gameObject;
        }
    }
}
