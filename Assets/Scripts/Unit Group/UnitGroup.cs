using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitGroup : MonoBehaviour
{
    public List<UnitCondition> unitList = new List<UnitCondition>();
    private List<Vector3> _points = new List<Vector3>();

    private FormationBase _formation;

    public FormationBase Formation
    {
        get
        {
            if (_formation == null) _formation = GetComponent<FormationBase>();
            return _formation;
        }
        set => _formation = value;
    }

    public float _rotateSpeed = 10f;

    private void Awake()
    {
        unitList = GetComponentsInChildren<UnitCondition>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Reset Unit List Position")]
    //Reset unit position
    public void ResetPosition()
    {
        _points = Formation.EvaluatePoints().ToList();
        for (int i = 0; i < unitList.Count; i++)
        {
            unitList[i].transform.position = transform.position + _points[i];
        }
    }

    //[ContextMenu("Move All Unit to Formation")]
    //public void MoveAllUnitToPosition()
    //{
    //    _points = Formation.EvaluatePoints().ToList();
    //    for (int i = 0; i < _points.Count; i++)
    //    {
    //        var unitAI = unitList[i].GetComponent<AI_Detection>();
    //        Vector3 targetPos = transform.position + _points[i];
    //        unitAI.targetPosition = targetPos;
    //        unitAI.MoveToPosition(targetPos);
    //    }
    //}

    public void MoveAllUnitToPosition(List<Vector3> pointList, Transform parentTransform)
    {
        _points = pointList;
        for (int i = 0; i < _points.Count; i++)
        {
            var unitAI = unitList[i].GetComponent<AI_Detection>();
            Vector3 targetPos = parentTransform.position + _points[i];
            unitAI.targetPosition = targetPos;
            unitAI.MoveToPosition(targetPos);
        }
    }
}
