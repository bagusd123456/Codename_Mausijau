using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AI_Detection : MonoBehaviour
{
    public bool isControlled = false;
    public enum DetectionState 
    { Idle, Moving, Attacking, 
        Patrol, Aggressive, Attacked,
        FindEnemies, MovingToTarget
    }
    public DetectionState currentState = DetectionState.Idle;

    private CharacterMovement characterMovement;
    private UnitCondition currentUnit;
    public float radius = 10f;
    public LayerMask layer;
    public LayerMask[] layerPriority;
    public bool targetInRange;

    public Collider[] detectedUnits;
    public List<Collider> sortedUnits;
    public UnitCondition closestTarget;

    public float minimumDistance = 0.2f;
    [HideInInspector] public Vector3 targetPosition;

    public float controlledDuration;
    public float delayTime;

    public float angle;

    [Header("Blackboard")]
    public float elapsedTime;

    public float currentDistance;
    public float lastDistance;

    public void Awake()
    {
        currentUnit = GetComponent<UnitCondition>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isControlled)
        {
            if (controlledDuration > 0)
            {
                controlledDuration -= Time.deltaTime;
                return;
            }
            else
            {
                isControlled = false;
            }
        }
        

        CheckSurrounding();

        //if (!targetInRange)
        //    closestTarget = null;
        //else
        //{
        //    if (closestTarget == null) return;
        //    currentDistance = Vector3.Distance(transform.position, closestTarget.transform.position);

        //    if (currentDistance > minimumDistance)
        //    {
        //        if(currentState != DetectionState.Moving)
        //        {
        //            currentState = DetectionState.Moving;

        //            float angle = Random.Range(0, 360);
        //            targetPosition = CharacterMovement.GetTargetPositon(closestTarget.transform, minimumDistance, angle);
        //            elapsedTime = 0;
        //        }
        //        else
        //        {
        //            if (currentState == DetectionState.Moving)
        //            {
        //                elapsedTime += Time.deltaTime;
        //                if (elapsedTime > 1f)
        //                {
        //                    if (currentDistance - minimumDistance < 2f)
        //                    {
        //                        currentState = DetectionState.Attacking;
        //                    }
        //                    else
        //                    {
        //                        float angle = Random.Range(0, 360);
        //                        targetPosition = CharacterMovement.GetTargetPositon(closestTarget.transform, minimumDistance, angle);
        //                        elapsedTime = 0;
        //                    }
        //                }
        //            }
        //        }
                
        //    }
        //    else
        //    {
        //        characterMovement._currentMovement = MovementStates.None;
        //        currentState = DetectionState.Attacking;
        //    }
        //}

        if(currentState == DetectionState.Idle)
        {
            characterMovement.SetAnimation(MovementStates.None);
        }

        else if (currentState == DetectionState.Attacking)
        {
            characterMovement.StopNavigation();
            characterMovement.SetAnimation(MovementStates.None);

            if (currentUnit != null && closestTarget != null)
            {
                currentDistance = Vector3.Distance(transform.position, closestTarget.transform.position);
                if (currentDistance < minimumDistance)
                {
                    if (closestTarget.isActiveAndEnabled)
                    {
                        closestTarget.GetComponent<AI_Detection>().currentState = DetectionState.FindEnemies;
                        currentUnit.Attack(closestTarget, currentUnit.unitData.baseDamage);
                    }
                    else
                        closestTarget = null;
                }
                else
                {
                    currentState = DetectionState.FindEnemies;
                }

                
            }
            else
                currentState = DetectionState.FindEnemies;
        }

        else if (currentState == DetectionState.Moving)
        {
            characterMovement.SetAnimation(MovementStates.Run);

            if (Vector3.Distance(transform.position, targetPosition) < 0.8f)
            {
                characterMovement.SetAnimation(MovementStates.None);
            }
        }

        else if (currentState == DetectionState.MovingToTarget)
        {
            characterMovement.SetAnimation(MovementStates.Run);
            characterMovement.MoveTo(targetPosition);
        }

        else if (currentState == DetectionState.FindEnemies)
        {
            if (closestTarget == null) return;

            currentDistance = Vector3.Distance(transform.position, closestTarget.transform.position);

            if (currentDistance > minimumDistance)
            {
                if (targetPosition == Vector3.zero)
                {
                    angle = Random.Range(0, 360);
                    //targetPosition = CharacterMovement.GetTargetPositon(closestTarget.transform, minimumDistance, angle);
                    lastDistance = currentDistance;
                }

                characterMovement.SetAnimation(MovementStates.Run);
                characterMovement.MoveTo(targetPosition);
                targetPosition = CharacterMovement.GetTargetPositon(closestTarget.transform, minimumDistance, angle);
                if (currentDistance < lastDistance)
                {
                    lastDistance = currentDistance;
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime is > .5f and < 1f)
                    {
                        float angle = Random.Range(0, 360);
                        targetPosition = CharacterMovement.GetTargetPositon(closestTarget.transform, minimumDistance, angle);
                    }
                    else if (elapsedTime > 1f)
                    {
                        if(detectedUnits.Length > 0)
                            closestTarget = detectedUnits[Random.Range(0, detectedUnits.Length)].GetComponent<UnitCondition>();
                        elapsedTime = 0;
                    }
                }
            }
            else
            {
                characterMovement.SetAnimation(MovementStates.None);

                targetPosition = Vector3.zero;
                lastDistance = 0;
                elapsedTime = 0;

                currentState = DetectionState.Attacking;
            }
        }
        controlledDuration = delayTime;
    }

    public void CheckSurrounding()
    {
        detectedUnits = Physics.OverlapSphere(transform.position, radius, layer);
        sortedUnits = detectedUnits.ToList();
        sortedUnits.Sort((x, y) => Vector3.Distance(transform.position, x.transform.position).CompareTo(Vector3.Distance(transform.position, y.transform.position)));

        if (detectedUnits.Length > 0 && closestTarget == null)
        {
            targetInRange = true;
            closestTarget = sortedUnits[0].GetComponent<UnitCondition>();
        }
        else
        {
            targetInRange = false;
        }
        
    }

    public void MoveToTarget()
    {
        if (closestTarget == null) return;
        currentDistance = Vector3.Distance(transform.position, closestTarget.transform.position);

        if (currentDistance > minimumDistance)
        {
            currentState = DetectionState.Moving;
        }

        else
        {
            currentState = DetectionState.Patrol;
        }
    }

    public void LookToTarget()
    {
        if (closestTarget == null) return;
        Vector3 direction = (closestTarget.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    //public void LookToTarget()
    //{
    //    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotateSpeed);

    //    _direction = (_moveTarget.WithNewY(transform.position.y) - transform.position).normalized;
    //    _lookRotation = Quaternion.LookRotation(_direction, Vector3.up);
    //}

    public void OnDrawGizmosSelected()
    {
        if(detectedUnits.Length > 0)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.white;

        if (closestTarget != null)
        {
            Gizmos.DrawLine(transform.position, closestTarget.transform.position);
            //Gizmos.DrawWireSphere(closestTarget.transform.position, radius);
        }

        if(closestTarget != null)
            Gizmos.DrawWireSphere(targetPosition, .5f);
        Gizmos.DrawWireSphere(transform.position, minimumDistance);
    }
}
