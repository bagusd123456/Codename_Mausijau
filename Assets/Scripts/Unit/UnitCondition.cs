using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUnitData
{
    public int health = 100;
    public int damage = 2;
    //public int baseArmor;
    public int moveSpeed = 5;

    [Space]
    public int sightRange = 2;

    public int attackSpeed = 2;
    public int attackRange = 2;
}

public class UnitCondition : MonoBehaviour
{
    public static Action OnUnitDeath;
    private CustomUnitData customUnitData;

    public BaseUnitData unitData;
    [Header("Unit Stats")]
    public int currentHealth;
    public bool isDead;

    [Header("Unit Attack")]
    public float timeBeforeAttack;

    void Awake()
    {
        InitUnitStats();
    }

    public void Update()
    {
        timeBeforeAttack -= Time.deltaTime;
    }

    public void InitUnitStats()
    {
        currentHealth = unitData.baseHealth;
        timeBeforeAttack = unitData.baseAttackSpeed;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;

            OnUnitDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void Attack(UnitCondition targetUnit, int damageAmount)
    {
        if (timeBeforeAttack <= 0)
        {
            timeBeforeAttack = unitData.baseAttackSpeed;
            targetUnit.TakeDamage(damageAmount);
            Debug.Log($"{targetUnit.name} Taken {damageAmount} Damage");
        }
    }
}
