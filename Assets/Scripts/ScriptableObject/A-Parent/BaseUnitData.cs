using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Create Unit")]
public class BaseUnitData : ScriptableObject
{
    public enum AttackType { Melee, Ranged };

    public int unitId = 000;
    public string unitName;

    [Header("Attribute Parameter")]
    public int baseHealth = 100;
    public int baseDamage = 2;
    //public int baseArmor;
    public int baseMoveSpeed = 5;

    //[Space]
    //public int baseSightRange = 2;

    [Header("Attack Parameter")]
    public AttackType attackType;
    public int baseAttackSpeed = 2;
    public int baseAttackRange = 2;

}
