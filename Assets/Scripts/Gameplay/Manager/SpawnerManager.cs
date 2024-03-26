using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnerManager : MonoBehaviour
{
    public List<Transform> spawnLocation = new List<Transform>();
    public List<WaveArmy> enemyWave = new List<WaveArmy>();
    public List<UnitCondition> spawnedEnemyUnit = new List<UnitCondition>();
    public int currentWaveIndex;

    public static SpawnerManager Instance { get; private set; }
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
    }

    [Button("SpawnWave")]
    public void SpawnWave()
    {
        //var currentWaveArmy = enemyWave.Find(x=> x.waveNumber == currentWaveIndex);
        var currentWaveArmy = enemyWave[currentWaveIndex];
        var tempList = new List<UnitCondition>();
        for (int i = 0; i < currentWaveArmy.enemyUnit.Count; i++)
        {
            var randomSpawnLocation = spawnLocation[UnityEngine.Random.Range(0, spawnLocation.Count)];

            var spawnedWave = Instantiate(currentWaveArmy.enemyUnit[i], randomSpawnLocation.position, Quaternion.identity, transform);
            spawnedWave.gameObject.SetActive(true);
            var unitArmy = spawnedWave.GetComponent<UnitArmy>();
            for (int j = 0; j < unitArmy.unitList.Count; j++)
            {
                tempList.Add(unitArmy.unitList[j]);
            }
        }

        GameManager.Instance.enemyUnit = tempList;
    }
}

[Serializable]
public class WaveArmy
{
    public int waveNumber;
    public List<UnitArmy> enemyUnit = new List<UnitArmy>();
}
