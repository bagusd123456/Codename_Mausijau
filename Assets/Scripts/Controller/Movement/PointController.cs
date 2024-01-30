using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PointController : MonoBehaviour
{
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

    [SerializeField]
    private List<NavMeshAgent> _spawnedUnits = new List<NavMeshAgent>();
    [SerializeField]
    private List<Vector3> _points = new List<Vector3>();
    public Transform _parent;

    [SerializeField] private float _unitSpeed = 2;
    public Vector3 _formationOffset;

    public UnitGroup unitGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePos();
        SetFormation();
    }

    public void GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                for (var i = 0; i < _spawnedUnits.Count; i++)
                {
                    //var ai_Detection = _spawnedUnits[i].GetComponent<AI_Detection>();

                    //ai_Detection.currentState = AI_Detection.DetectionState.Moving;
                    //ai_Detection.controlledDuration = 1f;
                    //_spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position, transform.position + _points[i], _unitSpeed * Time.deltaTime);
                }
                
                MoveParentTo(hit.point);
                _points = Formation.EvaluatePoints().ToList();
                //unitGroup.MoveAllUnitToPosition(_points, transform);
            }
            
        }
    }

    [Tooltip("Move this Unit Group to Target Position in Vector3")]
    public void MoveParentTo(Vector3 targetPos)
    {
        transform.position = targetPos;
        Formation.EvaluatePoints();
    }

    private void SetFormation()
    {
        _points = Formation.EvaluatePoints().ToList();

        for (var i = 0; i < _spawnedUnits.Count; i++)
        {
            //var ai_Detection = _spawnedUnits[i].GetComponent<AI_Detection>();
            //if (_spawnedUnits[i].isActiveAndEnabled && ai_Detection.currentState is AI_Detection.DetectionState.Idle or AI_Detection.DetectionState.Moving)
            //{
            //    _spawnedUnits[i].SetDestination(transform.position + _points[i] + _formationOffset);
            //    ai_Detection.targetPosition = transform.position + _points[i];
            //}
            //_spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position, transform.position + _points[i], _unitSpeed * Time.deltaTime);
        }
    }
}
