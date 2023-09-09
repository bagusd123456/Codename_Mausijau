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
                transform.position = hit.point;
                Formation.EvaluatePoints();
            }
            
        }
    }

    private void SetFormation()
    {
        _points = Formation.EvaluatePoints().ToList();

        for (var i = 0; i < _spawnedUnits.Count; i++)
        {
            _spawnedUnits[i].SetDestination(transform.position + _points[i]);
            //_spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position, transform.position + _points[i], _unitSpeed * Time.deltaTime);
        }
    }
}
