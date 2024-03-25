using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp_Button : MonoBehaviour
{
    public TMP_Text titleText_TMP;
    public TMP_Text descriptionText_TMP;
    public Button levelUp_Button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitButton(BaseUnitData unitData)
    {
        titleText_TMP.text = unitData.unitName;
        string statsInfo = $"Attack Damage: {unitData.baseAttackDamage}\n" +
                           $"Attack Range: {unitData.baseAttackRange}\n"+
                           $"Attack Speed: {unitData.baseAttackSpeed}\n"+
                           $"Health Point: {unitData.baseHealth}";
        descriptionText_TMP.text = statsInfo;
    }
}
