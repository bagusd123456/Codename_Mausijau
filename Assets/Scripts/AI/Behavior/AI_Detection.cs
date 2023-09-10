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
    public enum DetectionState { Idle, Moving, Attacking }
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
    public float currentDistance;
    private Vector3 targetPosition;

    public float controlledDuration;
    public float delayTime;

    public float elapsedTime;
    public float angle;
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

        if (!targetInRange)
            closestTarget = null;
        else
        {
            if (closestTarget == null) return;
            currentDistance = Vector3.Distance(transform.position, closestTarget.transform.position);

            if (currentDistance > minimumDistance)
            {
                if(currentState != DetectionState.Moving)
                {
                    currentState = DetectionState.Moving;

                    float angle = Random.Range(0, 360);
                    targetPosition = CharacterMovement.GetTargetPositon(closestTarget.transform, minimumDistance, angle);
                    elapsedTime = 0;
                }
                else
                {
                    if (currentState == DetectionState.Moving)
                    {
                        elapsedTime += Time.deltaTime;
                        if (elapsedTime > 1f)
                        {
                            if (currentDistance - minimumDistance < 2f)
                            {
                                currentState = DetectionState.Attacking;
                            }
                            else
                            {
                                float angle = Random.Range(0, 360);
                                targetPosition = CharacterMovement.GetTargetPositon(closestTarget.transform, minimumDistance, angle);
                                elapsedTime = 0;
                            }
                        }
                    }
                }
                
            }
            else
            {
                characterMovement._currentMovement = MovementStates.None;
                currentState = DetectionState.Attacking;
            }
        }

        if(currentState == DetectionState.Idle)
        {
            characterMovement.SetAnimation(MovementStates.None);
        }

        else if (currentState == DetectionState.Attacking)
        {
            characterMovement.SetAnimation(MovementStates.None);

            if (currentUnit != null && closestTarget != null)
                currentUnit.Attack(closestTarget, currentUnit.unitData.baseDamage);
            else
                currentState = DetectionState.Idle;
        }

        else if (currentState == DetectionState.Moving)
        {
            characterMovement.SetAnimation(MovementStates.Run);
            characterMovement.MoveTo(targetPosition);
        }
        controlledDuration = delayTime;
    }

    public void CheckSurrounding()
    {
        detectedUnits = Physics.OverlapSphere(transform.position, radius, layer);
        sortedUnits = detectedUnits.ToList();
        sortedUnits.Sort((x, y) => Vector3.Distance(transform.position, x.transform.position).CompareTo(Vector3.Distance(transform.position, y.transform.position)));

        if (detectedUnits.Length > 0)
        {
            targetInRange = true;
            closestTarget = sortedUnits[0].GetComponent<UnitCondition>();
        }
        else
        {
            targetInRange = false;
        }
        
    }

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
