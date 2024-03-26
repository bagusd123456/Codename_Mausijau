using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitArmy : MonoBehaviour
{
    public List<UnitCondition> unitList = new List<UnitCondition>();

    private void Awake()
    {
        unitList = new List<UnitCondition>(GetComponentsInChildren<UnitCondition>());
    }
}
