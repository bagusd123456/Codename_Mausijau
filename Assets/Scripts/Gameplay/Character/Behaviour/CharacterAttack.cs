using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using NaughtyAttributes;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private UnitCondition _unitCondition;
    public AnimationController animationController;

    public GameObject projectile;
    public Transform firePoint;

    private void Awake()
    {
        _unitCondition = GetComponent<UnitCondition>();
        animationController = GetComponent<AnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Transform targetTransform)
    {
        //animationController.CurrentState = MovementStates.Attack;
        //_unitCondition.GetComponent<CharacterMovement>().LookToTarget(targetTransform.position);
        if (_unitCondition.unitData.attackType == BaseUnitData.AttackType.Melee)
        {
            LaunchMelee(targetTransform);
        }
        else
        {
            LaunchProjectile(targetTransform.position);
        }
    }

    public void AttackMultiple(List<Transform> targetUnitList)
    {
        foreach (var item in targetUnitList)
        {
            Vector3 targetPos = item.position;
            LaunchProjectile(targetPos);
        }
    }

    public void LaunchMelee(Transform targetUnit)
    {
        if (targetUnit.TryGetComponent(out UnitCondition unit))
        {
            //Give damage to the target unit
            int dmgAmount = _unitCondition.unitData.baseAttackDamage;
            unit.TakeDamage(dmgAmount);

            //Alert other unit that this unit has been attacked
            unit.OnUnitAttacked?.Invoke(_unitCondition);

            //Debug.Log($"Unit: {unit.gameObject.name} " +
            //          $"\nTaken <color=red>{dmgAmount} damage...</color>");
        }
    }

    public void LaunchProjectile(Vector3 targetPos)
    {
        //Set the projectile to look at the target position
        firePoint.LookAt(targetPos);

        //Set projectile launch force
        float distance = Vector3.Distance(transform.position, targetPos);
        var projectileGO = Instantiate(projectile, firePoint.position, firePoint.rotation);
        projectileGO.GetComponent<Rigidbody>().AddForce(firePoint.forward * (Mathf.Sqrt(distance) * 2.2f) + firePoint.up * (Mathf.Sqrt(distance) * 2.1f), ForceMode.Impulse);

        //Set Projectile Behaviour properties
        var projectileBehaviour = projectileGO.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.projectileOwner = _unitCondition;
        int dmgAmount = _unitCondition.unitData.baseAttackDamage;
        projectileBehaviour.damage = dmgAmount;
    }
}
