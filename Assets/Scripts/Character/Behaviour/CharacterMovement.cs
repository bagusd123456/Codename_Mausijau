using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class CharacterMovement : MonoBehaviour
{
    private Transform currentTargetPosition;
    private Transform lastTargetPosition;

    private AnimationController animationController;

    private NavMeshAgent _agent;
    private Rigidbody _rb;

    public Vector3 _moveTarget = Vector3.zero;
    private Quaternion _lookRotation = Quaternion.identity;
    public Vector3 facingDirection = Vector3.zero;
    public bool _needToRotate = false;

    private float _rotateSpeed = 10f;
    public float _walkSpeed = 2.5f;
    public float _runSpeed = 4f;
    public float agentMoveSpeed;

    public MovementStates _currentMovement;
    public MovementStates CurrentMovement
    {
        get => _currentMovement;
        set
        {
            switch (value)
            {
                case MovementStates.Walk:
                    _agent.speed = _walkSpeed;
                    break;
                case MovementStates.Run:
                    _agent.speed = _runSpeed;
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
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleMovementState();
        HandleAnimation();

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

            if (Vector3.Dot(facingDirection, transform.forward) >= .99f)
            {
                //_agent.SetDestination(_moveTarget);
                animationController.CurrentState = CurrentMovement;

                _needToRotate = false;
            }
        }

    }

    public void HandleMovementState()
    {
        agentMoveSpeed = _agent.velocity.magnitude / _agent.speed;
        if (_agent.velocity.magnitude / _agent.speed >= 0.7f)
        {
            _currentMovement = MovementStates.Run;
        }
        else if (_agent.velocity.magnitude / _agent.speed >= 0.1f)
        {
            _currentMovement = MovementStates.Run;
        }
        else if (_agent.velocity.magnitude / _agent.speed < 0.1f)
        {
            _currentMovement = MovementStates.None;
        }

        if (animationController.isActiveAndEnabled)
            animationController.CurrentState = CurrentMovement;
    }

    public void HandleAnimation()
    {
        bool flip = facingDirection.x < 0;
        animationController.FlipSprite(flip);
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
        facingDirection = (targetPos.WithNewY(transform.position.y) - transform.position).normalized;
        _lookRotation = Quaternion.LookRotation(facingDirection, Vector3.up);
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
