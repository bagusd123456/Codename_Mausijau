using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public Transform target;
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (target != null) return;

        if (other.TryGetComponent(out UnitCondition targetUnit))
        {
            //Avoid hitting another unit
            target = targetUnit.transform;

            if (!targetUnit.isDead)
            {
                //If unit is not dead then take damage
                targetUnit.TakeDamage(damage);
                //Debug.Log($"Unit: {other.name} " +
                //          $"\nTaken <color=red>{damage} damage...</color>");
            }
        }

        //Avoid hitting another projectile
        if (!other.CompareTag("Projectile"))
            Destroy(gameObject);
    }
}