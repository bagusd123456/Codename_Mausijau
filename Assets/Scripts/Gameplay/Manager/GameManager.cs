using System;
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Yarn.Unity;

public enum State {NotInitialized, Initialized, Busy, Error}
public class GameManager : MonoBehaviour
{
    public static Action<Condition> OnGameStateChange;

    public enum Condition {Idle, Running, Paused, Busy, AlliedWin, EnemyWin, Draw}

    private bool _gameFinished = false;
    public bool showLevelUpOnStart = true;
    public bool startGameOnStart = false;
    public bool forceLoseCondition = false;
    public bool showFinalCutscene = false;
    public GameObject finalCutscene;
    public int enemyInsideBaseLimit = 10;
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

    public List<UnitArmy> alliedArmyList;

    public GameObject winPanel;
    public GameObject losePanel;

    public TransitionSettings transition;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        StartCoroutine(InitGame());
    }

    private void OnEnable()
    {
        UnitCondition.OnUnitDeath += RemoveUnit;
        UnitCondition.OnEnemyArrivedBase += EnemyArrivedBaseHandler;
        OnGameStateChange += GameStateChangeHandler;
        LevelUpPanelView.OnLevelUpTriggered += CheckNextCharacterLevelUp;
    }

    private void OnDisable()
    {
        UnitCondition.OnUnitDeath -= RemoveUnit;
        UnitCondition.OnEnemyArrivedBase -= EnemyArrivedBaseHandler;
        OnGameStateChange -= GameStateChangeHandler;
        LevelUpPanelView.OnLevelUpTriggered -= CheckNextCharacterLevelUp;
    }

    private void CheckNextCharacterLevelUp(int index)
    {
        if (index < alliedArmyList.Count)
        {
            gameState = Condition.Paused;
            ShowLevelUpPanel();
        }
        else
        {
            var levelUpPanel = FindObjectOfType<LevelUpPanelView>();
            levelUpPanel.currentArmyIndex = 0;
            levelUpPanel.gameObject.SetActive(false);

            Time.timeScale = 1f;
            gameState = Condition.Running;
        }
    }

    private void GameStateChangeHandler(Condition condition)
    {
        
    }

    public void ShowLevelUpPanel()
    {
        EventSystem.current.SetSelectedGameObject(null);
        var levelUpPanel = FindObjectOfType<LevelUpPanelView>(true);
        
        if (levelUpPanel != null)
        {
            levelUpPanel.gameObject.SetActive(true);
            levelUpPanel.targetArmy = alliedArmyList[levelUpPanel.currentArmyIndex];
            BaseUnitData unitDataToUpgrade = levelUpPanel.unitDataList.Find(x => x == levelUpPanel.targetArmy.unitList[0].unitData);

            if (unitDataToUpgrade == null)
            {
                foreach (var baseUnitData in levelUpPanel.targetArmy.unitList[0].unitData.levelUpUnitList)
                {
                    unitDataToUpgrade = levelUpPanel.unitDataList.Find(x => x == baseUnitData);
                }
            }

            levelUpPanel.unitData = unitDataToUpgrade;
            levelUpPanel.InitButton();
            levelUpPanel.currentArmyIndex++;
        }
    }

    [Button()]
    public void TriggerWinCondition()
    {
        if (forceLoseCondition)
        {
            TriggerLoseCondition();
            return;
        }

        //var winPanel = FindObjectOfType<WinCondition_PanelView>(true);

        if (winPanel != null)
        {
            winPanel.gameObject.SetActive(true);
        }

        gameState = Condition.AlliedWin;
        Debug.Log($"Ally Win....");
    }

    [Button()]
    public void TriggerLoseCondition()
    {
        if (gameState == Condition.EnemyWin) return;

        gameState = Condition.EnemyWin;
        Debug.Log($"Enemy Win....");

        //var losePanel = FindObjectOfType<LoseCondition_PanelView>(true);
        if (showFinalCutscene)
        {
            ShowFinalCutscene();
            return;
        }
        if (losePanel != null)
        {
            losePanel.gameObject.SetActive(true);
        }
    }

    public void ShowFinalCutscene()
    {
        TransitionManager.Instance().Transition("Cutscene_Level3Final", transition, 0f);
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

            unit.currentHealth = unit.unitData.baseHealth;
            unit.isDead = false;
        }
        unitChecked = true;

        yield return new WaitUntil(() => unitChecked);

        SpawnerManager.Instance.SpawnWave();

        if (showLevelUpOnStart)
            ShowLevelUpPanel();

        if (startGameOnStart)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        gameState = Condition.Running;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    //Get specific unit and delete it from the list
    public void RemoveUnit(UnitCondition unit)
    {
        if (unit.gameObject.layer == LayerMask.NameToLayer("AlliedUnit"))
        {
            alliedUnit.Remove(unit);
        }
        if (unit.gameObject.layer == LayerMask.NameToLayer("EnemyUnit"))
        {
            enemyUnit.Remove(unit);
        }

        if (alliedUnit.Count == 0)
        {
            TriggerLoseCondition();
        }

        if (enemyUnit.Count == 0)
        {
            //ShowLevelUpPanel();
            int spawnIndex = SpawnerManager.Instance.currentWaveIndex;
            //var canSpawnWave = SpawnerManager.Instance.enemyWave.Exists(x => x.waveNumber == spawnIndex);
            var canSpawnWave = SpawnerManager.Instance.enemyWave.Count > spawnIndex;
            if (canSpawnWave)
            {
                SpawnerManager.Instance.SpawnWave();
                SpawnerManager.Instance.currentWaveIndex++;
            }
            else
            {
                TriggerWinCondition();
            }
        }
    }

    public void EnemyArrivedBaseHandler(UnitCondition unit)
    {
        enemyUnit.Remove(unit);
        unit.Dead();
        enemyInsideBaseLimit--;
        if (enemyInsideBaseLimit <= 0)
        {
            TriggerLoseCondition();
        }
    }
}
