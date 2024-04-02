using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp_Button : MonoBehaviour
{
    public TMP_Text titleText_TMP;
    public TMP_Text descriptionText_TMP;
    public Image levelUp_Image;
    public Button levelUp_Button;

    public BaseUnitData unitData;
    private void OnEnable()
    {
        //InitButton();
    }

    private void OnDisable()
    {
        
    }

    public void InitButton(BaseUnitData unitData)
    {
        //titleText_TMP.text = unitData.unitName;
        //string statsInfo = $"Attack Damage: {unitData.baseAttackDamage}\n" +
        //                   $"Attack Range: {unitData.baseAttackRange}\n"+
        //                   $"Attack Speed: {unitData.baseAttackSpeed}\n"+
        //                   $"Health Point: {unitData.baseHealth}";
        //descriptionText_TMP.text = statsInfo;

        levelUp_Image.sprite = unitData.levelUpCardSprite;
        var spriteState = new SpriteState();
        spriteState.highlightedSprite = unitData.levelUpCardSpriteHighlighted;
        spriteState.pressedSprite = unitData.levelUpCardSpriteHighlighted;
        spriteState.selectedSprite = unitData.levelUpCardSpriteHighlighted;
        levelUp_Button.spriteState = spriteState;

    }
}
