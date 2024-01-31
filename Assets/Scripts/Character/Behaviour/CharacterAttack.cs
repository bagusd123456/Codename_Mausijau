using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private UnitCondition _unitCondition;

    public GameObject projectile;
    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        _unitCondition = GetComponent<UnitCondition>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Transform targetTransform)
    {
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
            int dmgAmount = _unitCondition.unitData.baseAttackDamage;

            unit.TakeDamage(dmgAmount);
            //Debug.Log($"Unit: {unit.gameObject.name} " +
            //          $"\nTaken <color=red>{dmgAmount} damage...</color>");
        }
    }

    public void LaunchProjectile(Vector3 targetPos)
    {
        firePoint.LookAt(targetPos);

        int dmgAmount = _unitCondition.unitData.baseAttackDamage;
        float distance = Vector3.Distance(transform.position, targetPos);
        var projectileGO = Instantiate(projectile, firePoint.position, firePoint.rotation);
        projectileGO.GetComponent<ProjectileBehaviour>().damage = dmgAmount;

        projectileGO.GetComponent<Rigidbody>().AddForce(firePoint.forward * (Mathf.Sqrt(distance) * 2) + firePoint.up * (Mathf.Sqrt(distance) * 2), ForceMode.Impulse);
    }
}
