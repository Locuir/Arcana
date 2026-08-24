using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class WaveEnemy
    {
        public GameObject EnemyPrefab;
        public int Amount;
    }

    [System.Serializable]
    public class WaveData
    {
        public WaveEnemy[] Enemies;
    }

    public static WaveManager Instance;

    public WaveData[] Waves;

    public enum WavePhase
    {
        KillMonsters,
        PrepareLoadout
    }

    [Header("Wave Settings")]
    public int currentWave = 1;

    [Header("Phase Settings")]
    public float prepareTime = 30f;

    public WavePhase currentPhase;

    public float CurrentPhaseTime { get; private set; }
    public int CurrentEnemies { get; private set; }
    public int MaxEnemies { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartWave();
    }

    void StartWave()
    {
        currentPhase = WavePhase.KillMonsters;

        WaveData wave = Waves[currentWave - 1];

        MaxEnemies = 0;

        foreach (WaveEnemy enemy in wave.Enemies)
        {
            MaxEnemies += enemy.Amount;
        }

        CurrentEnemies = MaxEnemies;

        Debug.Log("WAVE " + currentWave + " STARTED");

        if (MusicManager.Instance != null)
        {
            if (currentWave % 5 == 0)
                MusicManager.Instance.PlayBoss();
            else if (currentWave >= 4)
                MusicManager.Instance.PlayIntense();
            else
                MusicManager.Instance.PlayNormal();
        }

        MonsterSpawner spawner =
            FindObjectOfType<MonsterSpawner>();

        if (spawner == null)
        {
            Debug.LogError("MONSTER SPAWNER NOT FOUND!");
            return;
        }

        MonsterSpawner.EnemySpawnData[] spawnData =
            new MonsterSpawner.EnemySpawnData[
                wave.Enemies.Length
            ];

        for (int i = 0; i < wave.Enemies.Length; i++)
        {
            spawnData[i] =
                new MonsterSpawner.EnemySpawnData();

            spawnData[i].EnemyPrefab =
                wave.Enemies[i].EnemyPrefab;

            spawnData[i].Amount =
                wave.Enemies[i].Amount;
        }

        spawner.SpawnWave(spawnData);
    }

    public void EnemyKilled()
    {
        CurrentEnemies--;

        if (CurrentEnemies <= 0)
        {
            CurrentEnemies = 0;

            WaveCompleted();
        }
    }

    public void WaveCompleted()
    {
        Debug.Log(
            "Wave " +
            currentWave +
            " Completed!"
        );

        StartCoroutine(PreparePhase());
    }

    IEnumerator PreparePhase()
    {
        currentPhase =
            WavePhase.PrepareLoadout;

        CurrentPhaseTime =
            prepareTime;

        Debug.Log(
            "PREPARE YOUR LOADOUT"
        );

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayPrepare();

        while (CurrentPhaseTime > 0)
        {
            CurrentPhaseTime -=
                Time.deltaTime;

            yield return null;
        }

        NextWave();
    }

    void NextWave()
    {
        currentWave++;

        if (currentWave > Waves.Length)
        {
            Debug.Log("ALL WAVES COMPLETED!");
            return;
        }

        StartWave();
    }
}