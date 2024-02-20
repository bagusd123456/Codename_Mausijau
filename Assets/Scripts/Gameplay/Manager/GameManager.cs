using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public enum State {NotInitialized, Initialized, Busy, Error}
public class GameManager : MonoBehaviour
{
    public static Action<Condition> OnGameStateChange;

    public enum Condition {Idle, Running, Paused, Busy, AlliedWin, EnemyWin, Draw}

    [ReadOnly]
    public Condition s_gameState;
    public Condition gameState
    {
        get
        {
            if (s_gameState == Condition.Running)
            {
                if (alliedUnit.Count == 0 && enemyUnit.Count > 0)
                {
                    s_gameState = Condition.EnemyWin;
                }
                else if (enemyUnit.Count == 0 && alliedUnit.Count > 0)
                {
                    s_gameState = Condition.AlliedWin;
                }
                else if (alliedUnit.Count == 0 && enemyUnit.Count == 0)
                {
                    s_gameState = Condition.Draw;
                }
            }
            
            return s_gameState;
        }
        private set
        {
            s_gameState = value;
            OnGameStateChange?.Invoke(s_gameState);
        }
    }

    public List<UnitCondition> alliedUnit;
    public List<UnitCondition> enemyUnit;

    private void Awake()
    {
        StartCoroutine(InitGame());
    }

    private void OnEnable()
    {
        OnGameStateChange += GameStateChangeHandler;
    }

    private void GameStateChangeHandler(Condition condition)
    {
        Debug.Log($"GameState set to: {condition}");
    }

    public IEnumerator InitGame()
    {
        //Debug.Log($"Checking Units...");
        bool unitChecked = false;
        var units = FindObjectsOfType<UnitCondition>();
        foreach (var unit in units)
        {
            if (unit.gameObject.layer == LayerMask.NameToLayer("AlliedUnit"))
            {
                alliedUnit.Add(unit);
            }
            else if (unit.gameObject.layer == LayerMask.NameToLayer("EnemyUnit"))
            {
                enemyUnit.Add(unit);
            }

            //unit.currentHealth = unit.unitData.baseHealth;
        }
        unitChecked = true;

        yield return new WaitUntil(() => unitChecked);
        gameState = Condition.Running;
        //Debug.Log($"Unit Check Finished....");
    }

    public void FixedUpdate()
    {
        if (gameState == Condition.Running)
        {
            for (int i = 0; i < alliedUnit.Count; i++)
            {
                var unit = alliedUnit[i];
                if (unit.isDead)
                {
                    alliedUnit.Remove(unit);
                }
            }

            for (int i = 0; i < enemyUnit.Count; i++)
            {
                var unit = enemyUnit[i];
                if (unit.isDead)
                {
                    enemyUnit.Remove(unit);
                }
            }
        }
        else
        {
            //Debug.Log($"Winner is: {gameState}");
        }
        //while (gameState == Condition.Running)
        //{
        //    foreach (var unit in alliedUnit)
        //    {
        //        if (unit.isDead)
        //        {
        //            alliedUnit.Remove(unit);
        //        }
        //    }

        //    foreach (var unit in enemyUnit)
        //    {
        //        if (unit.isDead)
        //        {
        //            enemyUnit.Remove(unit);
        //        }
        //    }
        //}
        
    }
}
