using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    private Transform currentTargetPosition;
    private Transform lastTargetPosition;

    private AnimationController animationController;

    private NavMeshAgent _agent;

    private Vector3 _moveTarget = Vector3.zero;
    private Quaternion _lookRotation = Quaternion.identity;
    private Vector3 _direction = Vector3.zero;
    public bool _needToRotate = false;

    private float _rotateSpeed = 10f;
    public float _walkSpeed = 2.5f;
    public float _runSpeed = 4f;

    public MovementStates _currentMovement;
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

    /// <summary>
    /// Returns true if the agent is in the middle of pathfinding or the agent has a remaining distance greater than 1/2 a meter (0.5f)
    /// </summary>
    public bool IsNavigating => _agent.pathPending || _agent.remainingDistance > .25f;

    void Start()
    {
        //UnitCondition.OnUnitDeath += ()=> _agent.isStopped = true;
        animationController = GetComponent<AnimationController>();
        //Register listener events for inputs

        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Confirm that the player is done navigating
        if (!_needToRotate && !IsNavigating && _currentMovement != MovementStates.None)
        {
            StopNavigation();
        }
        //Navigation is likely starting
        else if (_needToRotate)
        {
            _agent.enabled = true;
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotateSpeed);

            if (Vector3.Dot(_direction, transform.forward) >= .99f)
            {
                _agent.SetDestination(_moveTarget);
                animationController.CurrentState = CurrentMovement;

                _needToRotate = false;
            }
        }
    }

    /// <summary>
    /// Called when the player does a double left mouse button click
    /// </summary>
    private void Run(CallbackContext context)
    {
        CurrentMovement = MovementStates.Run;
        animationController.CurrentState = CurrentMovement;
    }

    /// <summary>
    /// Called when the player does a single left mouse button click
    /// </summary>
    private void Walk(CallbackContext context)
    {
        if (currentTargetPosition == lastTargetPosition)
        {
            return;
        }

        if (NavMesh.SamplePosition(ObjectSelector.currentTargetPosition.position, out NavMeshHit navPos, .25f, 1 << 0))
        {
            //Stop navigating
            StopNavigation();

            _moveTarget = navPos.position;

            //Calculate rotation direction
            _direction = (_moveTarget.WithNewY(transform.position.y) - transform.position).normalized;
            _lookRotation = Quaternion.LookRotation(_direction, Vector3.up);
            _needToRotate = true;

            //Set the speed and ready the animation
            CurrentMovement = MovementStates.Walk;
        }
    }

    public void MoveTo(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    /// <summary>
    /// Stops the player from moving.
    /// </summary>
    public void StopNavigation()
    {
        //_agent.enabled = false;
        _agent.SetDestination(transform.position);
        CurrentMovement = MovementStates.None;
        animationController.CurrentState = CurrentMovement;
    }

    public void SetAnimation(MovementStates moveState)
    {
        animationController.CurrentState = moveState;
    }

    public void LookToTarget(Vector3 targetPos)
    {
        _direction = (targetPos.WithNewY(transform.position.y) - transform.position).normalized;
        _lookRotation = Quaternion.LookRotation(_direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotateSpeed);
    }

    public static Vector3 GetTargetPositon(Transform center, float radius = 2f, float angle = 1f)
    {
        Vector3 pos = new Vector3();

        //pos.x = center.position.x + (radius * Mathf.Cos(angle / (180f / Mathf.PI)));
        //pos.y = center.position.y;
        //pos.z = center.position.z + (radius * Mathf.Sin(angle / (180f / Mathf.PI)));

        pos.x = center.position.x + Mathf.Cos(angle) * radius;
        pos.y = center.position.y;
        pos.z = center.position.z + Mathf.Sin(angle) * radius;

        return pos;
    }
}
