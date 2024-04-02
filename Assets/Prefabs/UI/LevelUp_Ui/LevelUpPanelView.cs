using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanelView : MonoBehaviour
{
    public static Action<int> OnLevelUpTriggered;
    public List<BaseUnitData> unitDataList;
    public BaseUnitData unitData;
    public List<LevelUp_Button> levelUpButtonList = new List<LevelUp_Button>();
    public List<UnitArmy> alliedToLevelUp = new List<UnitArmy>();
    public UnitArmy targetArmy;

    public int currentArmyIndex;
    public void Show()
    {
        gameObject.SetActive(true);
    }

    [ContextMenu("Init Button")]
    public void InitButton()
    {
        for (int i = 0; i < levelUpButtonList.Count; i++)
        {
            if (i >= unitData.levelUpUnitList.Count)
            {
                levelUpButtonList[i].gameObject.SetActive(false);
                continue;
            }

            var levelUpData = unitData.levelUpUnitList[i];
            levelUpButtonList[i].InitButton(unitData.levelUpUnitList[i]);

            levelUpButtonList[i].levelUp_Button.onClick.RemoveAllListeners();
            levelUpButtonList[i].levelUp_Button.onClick.AddListener(()=> TriggerLevelUp(levelUpData));

            levelUpButtonList[i].gameObject.SetActive(true);
        }
    }

    public void TriggerLevelUp(BaseUnitData targetUnitData)
    {
        foreach (var unitCondition in targetArmy.unitList)
        {
            unitCondition.unitData = targetUnitData;
            unitCondition.ChangeCharacterStanceFromUnitData();
            unitCondition.InitUnitStats();
        }

        OnLevelUpTriggered?.Invoke(currentArmyIndex);
    }
}
