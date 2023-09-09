using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AI_Detection : MonoBehaviour
{
    public UnitCondition currentUnit;
    public float radius = 10f;
    public LayerMask layer;
    public bool targetInRange;

    public Collider[] detectedUnits;
    public List<Collider> sortedUnits;
    public UnitCondition closestTarget;

    public void Awake()
    {
        currentUnit = GetComponent<UnitCondition>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckSurrounding();

        if (!targetInRange)
            closestTarget = null;
        else
        {
            currentUnit.Attack(closestTarget, currentUnit.unitData.baseDamage);
        }
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

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
