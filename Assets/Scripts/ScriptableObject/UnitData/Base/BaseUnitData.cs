using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Unit Data", menuName = "Create Unit")]
public class BaseUnitData : ScriptableObject
{
    public enum AttackType { Melee, Ranged };

    public int unitId = 000;
    public string unitName;

    [Header("Attribute Parameter")]
    public int baseHealth = 100;
    //public int baseArmor;
    public float baseMoveSpeed = 5;

    //[Space]
    //public int baseSightRange = 2;

    [Header("Attack Parameter")]
    public AttackType attackType;
    public int baseAttackDamage = 2;
    public float baseAttackSpeed = 2;
    public float baseAttackRange = 2;
    public CharacterStance characterStance;

    [Header("Level Up Parameter")]
    public List<BaseUnitData> levelUpUnitList = new List<BaseUnitData>();

    [Header("Character Reference")]
    public GameObject characterPrefab;
}
