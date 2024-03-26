using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using DamageNumbersPro;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitCondition : MonoBehaviour
{
    public static Action<UnitCondition> OnEnemyArrivedBase;
    public static Action<UnitCondition> OnUnitDeath;
    public Action<UnitCondition> OnUnitAttacked;

    private Outline outline;
    [Header("Unit Properties")]
    private CustomUnitData customUnitData;
    public BaseUnitData unitData;
    public DamageNumber damageNumberPrefab;

    [Header("Unit Stats")]
    public int currentHealth;
    public bool isDead;
    [HideInInspector] 
    public bool isSelected;

    public UnitArmy unitParentArmy;

    [Header("Unit Attack")]
    public float timeBeforeAttack;

    void Awake()
    {
        unitParentArmy = GetComponentInParent<UnitArmy>();

        outline = GetComponent<Outline>();
        InitUnitStats();
    }

    public void Update()
    {
        if(timeBeforeAttack > 0)
            timeBeforeAttack -= Time.deltaTime;
    }

    [ContextMenu("Change Character Stance")]
    public void ChangeCharacterStanceFromUnitData()
    {
        var animController = GetComponent<AnimationController>();
        animController.SetCharacterStance(unitData.characterStance);
    }

    [Button()]
    public void InitCharacterAnim()
    {
        var animController = GetComponent<AnimationController>();

        animController.ChangeCharacterAnimator(unitData.characterPrefab);
    }

    public void InitUnitStats()
    {
        currentHealth = unitData.baseHealth;
        timeBeforeAttack = unitData.baseAttackSpeed;
    }

    public void TakeDamage(int amount)
    {
        SpawnDamageNumber(amount);

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;

            OnUnitDeath?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    public void Dead()
    {
        currentHealth = 0;
        isDead = true;
        gameObject.SetActive(false);
    }

    public void Attack(UnitCondition targetUnit, int damageAmount)
    {
        if (timeBeforeAttack <= 0)
        {
            timeBeforeAttack = unitData.baseAttackSpeed;
            targetUnit.TakeDamage(damageAmount);
            //Debug.Log($"{targetUnit.name} Taken {damageAmount} Damage");
        }
    }

    public void SpawnDamageNumber(int number)
    {
        var GO = damageNumberPrefab.Spawn(transform.position + Vector3.up * 0.2f);
        GO.number = number;
        if(gameObject.layer == LayerMask.NameToLayer("AlliedUnit"))
            GO.SetColor(Color.blue);
        else
            GO.SetColor(Color.red);
    }

    public bool IsSelected()
    {
        outline.enabled = isSelected;
        return isSelected;
    }

    private void OnDrawGizmosSelected()
    {
        float currentRadius = 0f;

        if (unitData != null)
        {
            currentRadius = unitData.baseAttackRange;
            Gizmos.DrawWireSphere(transform.position, currentRadius);
        }
    }
}

public class CustomUnitData : BaseUnitData
{

}