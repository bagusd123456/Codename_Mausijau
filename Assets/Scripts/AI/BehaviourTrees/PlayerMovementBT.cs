using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using MBT;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovementBT : MonoBehaviour
{
    public MBTExecutor btExecutor;
    public Blackboard blackboard;
    private InputControls _inputMapping;

    private Camera _camera;
    private NavMeshAgent _agent;
    public CharacterMovement characterMovement;

    private Vector3 _moveTarget = Vector3.zero;
    private Quaternion _lookRotation = Quaternion.identity;
    private Vector3 _direction = Vector3.zero;
    private bool _needToRotate = false;

    public TransformReference targetTransform;
    public Transform mouseTargetTransform;

    private MovementStates _currentMovement;
    public MovementStates CurrentMovement
    {
        get => _currentMovement;
        set
        {
            switch (value)
            {
                case MovementStates.Walk:
                    _agent.speed = 2.5f;
                    break;
                case MovementStates.Run:
                    _agent.speed = 4f;
                    break;
            }

            _currentMovement = value;
        }
    }

    public bool IsNavigating => _agent.pathPending || _agent.remainingDistance > .25f;

    private void Awake() => _inputMapping = new InputControls();

    private void OnEnable() => _inputMapping.Enable();
    private void OnDisable() => _inputMapping.Disable();

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;

        //_inputMapping.Default.Walk.performed += Walk;
        //_inputMapping.Default.Run.performed += Run;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Run(InputAction.CallbackContext context)
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            
        }
    }

    private void Walk(InputAction.CallbackContext context)
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navPos, .25f, 1 << 0))
            {
                //Stop navigating
                StopNavigation();

                //Calculate rotation direction
                _direction = (_moveTarget.WithNewY(transform.position.y) - transform.position).normalized;
                _lookRotation = Quaternion.LookRotation(_direction, Vector3.up);
                _needToRotate = true;

                //Set the speed and ready the animation
                characterMovement.CurrentMovement = MovementStates.Walk;

                if (IsNavigating && Vector3.Dot(_direction, transform.forward) >= 0.25f)
                {
                    //_agent.SetDestination(_moveTarget);
                    blackboard.GetVariable<TransformVariable>("waypointTarget").Value = mouseTargetTransform;
                    btExecutor.monoBehaviourTree.Restart();
                    btExecutor.monoBehaviourTree.Tick();
                }
            }
        }
    }

    private void StopNavigation()
    {
        //_agent.SetDestination(transform.position);
        //CurrentMovement = MovementStates.None;
        //animationController.CurrentState = CurrentMovement;
    }
}
