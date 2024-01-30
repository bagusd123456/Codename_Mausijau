using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent(out UnitCondition targetUnit))
        {
            if (!targetUnit.isDead)
            {
                targetUnit.TakeDamage(damage);
            }
        }
        if(!other.CompareTag("Projectile"))
            Destroy(gameObject);
    }
}
