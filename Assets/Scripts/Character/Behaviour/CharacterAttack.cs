using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private UnitCondition _unitCondition;

    public GameObject projectile;
    public Transform firePoint;
    public Transform target;

    public List<Transform> multipleTarget;
    // Start is called before the first frame update
    void Start()
    {
        _unitCondition = GetComponent<UnitCondition>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button("Trigger Attack")]
    public void Attack()
    {
        LaunchProjectile(target.position);
    }

    [Button("Trigger Attack Multiple")]
    public void AttackMultiple()
    {
        for (int i = 0; i < multipleTarget.Count; i++)
        {
            Vector3 targetPos = multipleTarget[i].position;
            LaunchProjectile(targetPos, multipleTarget[i]);
        }
    }

    public void LaunchProjectile(Vector3 targetPos, Transform definedTarget = null)
    {
        float distance = Vector3.Distance(transform.position, targetPos);
        var projectileGO = Instantiate(projectile, firePoint.position, firePoint.rotation);
        projectileGO.GetComponent<ProjectileBehaviour>().damage = _unitCondition.unitData.baseAttackDamage;

        if(definedTarget == null)
            firePoint.LookAt(target.position);
        else
            firePoint.LookAt(definedTarget);

        projectileGO.GetComponent<Rigidbody>().AddForce(firePoint.forward * Mathf.Sqrt(distance) * 2 + firePoint.up * Mathf.Sqrt(distance) * 2, ForceMode.Impulse);
    }
}
