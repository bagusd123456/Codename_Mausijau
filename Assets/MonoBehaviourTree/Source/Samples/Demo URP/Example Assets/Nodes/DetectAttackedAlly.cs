using System.Collections;
using System.Collections.Generic;
using MBT;
using UnityEngine;

public class DetectAttackedAlly : Service
{
    private bool listenerInitialized;
    public TransformReference selfCondition = new TransformReference(VarRefMode.DisableConstant);
    public TransformReference commandTargetReference = new TransformReference(VarRefMode.DisableConstant);

    private bool foundListener;
    public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);
    public GameObjectReference enemyGameObject = new GameObjectReference(VarRefMode.DisableConstant);
    public BoolReference onCommand = new BoolReference();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Task()
    {
        
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

    //private void OnEnable()
    //{
    //    var alliedUnit = selfCondition.Value.GetComponent<UnitCondition>().unitArmy;
    //    if (alliedUnit != null)
    //    {
    //        //Debug.LogWarning($"Condition Registered...");
    //        foreach (var allyCondition in alliedUnit)
    //        {
    //            allyCondition.OnUnitAttacked += attackerUnit =>
    //            {
    //                OnAllyAttacked(attackerUnit);
    //            };
    //        }
    //    }

    //    if (selfCondition.Value.TryGetComponent(out UnitCondition unit))
    //    {
    //        unit.OnUnitAttacked += attackerUnit =>
    //        {
    //            onCommand.Value = false;
    //            commandTargetReference.Value = null;

    //            OnAllyAttacked(attackerUnit);
    //        };
    //    }
    //}

    //private void OnDisable()
    //{
    //    foundListener = false;

    //    var alliedUnit = selfCondition.Value.GetComponent<UnitCondition>().unitArmy;
    //    if (alliedUnit != null)
    //    {
    //        //Debug.LogWarning($"Condition Not Registered...");
    //        foreach (var allyCondition in alliedUnit)
    //        {
    //            allyCondition.OnUnitAttacked -= attackerUnit =>
    //            {
    //                OnAllyAttacked(attackerUnit);
    //            };
    //        }
    //    }

    //    if (selfCondition.Value.TryGetComponent(out UnitCondition unit))
    //    {
    //        unit.OnUnitAttacked -= attackerUnit =>
    //        {
    //            OnAllyAttacked(attackerUnit);
    //        };
    //    }
    //}
}
