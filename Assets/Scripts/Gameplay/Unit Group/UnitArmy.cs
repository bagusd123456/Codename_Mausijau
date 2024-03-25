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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
