using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileBehaviour : MonoBehaviour
{
    public UnitCondition projectileOwner;

    public Transform target;
    public int damage;
    public LayerMask enemyLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (target != null)
        {
            //return;
        }

        if (other.TryGetComponent(out UnitCondition targetUnit))
        {
            //Hit only enemy unit layermask
            bool isEnemy = enemyLayerMask == (enemyLayerMask | (1 << other.gameObject.layer));

            bool isSelf = other.gameObject == projectileOwner.gameObject;

            //Avoid hitting another unit
            target = targetUnit.transform;

            if (!targetUnit.isDead && isEnemy)
            {
                //If unit is not dead then take damage
                targetUnit.TakeDamage(damage);

                //Alert other unit that this unit has been attacked
                if (projectileOwner != null)
                {
                    targetUnit.OnUnitAttacked?.Invoke(projectileOwner);

                }
                //Debug.Log($"Unit: {other.name} " +
                //          $"\nTaken <color=red>{damage} damage...</color>");

                Destroy(gameObject);
                return;
            }
        }

        ////Avoid hitting another projectile
        //if (!other.CompareTag("Projectile"))
        //    Destroy(gameObject);
    }
}
