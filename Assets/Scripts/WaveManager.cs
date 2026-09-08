using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public System.Action WaveCompletedEvent;

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
    public PlayerStats stats;



    [Header("Wave Voice Lines")]
    public AudioSource VoiceAudioSource;
    public AudioClip Wave2Voice;
    public AudioClip Wave3Voice;
    public AudioClip Wave4Voice;
    public AudioClip Wave5Voice;

    public float CurrentPhaseTime { get; private set; }
    public int CurrentEnemies { get; private set; }
    public int MaxEnemies { get; private set; }

    private bool waveCompleted;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartWave();
    }

    private void PlayWaveVoiceLine(int wave)
    {
        if (VoiceAudioSource == null)
            return;

        AudioClip clip = null;

        switch (wave)
        {
            case 2:
                clip = Wave2Voice;
                break;

            case 3:
                clip = Wave3Voice;
                break;

            case 4:
                clip = Wave4Voice;
                break;

            case 5:
                clip = Wave5Voice;
                break;
        }

        if (clip != null)
            VoiceAudioSource.PlayOneShot(clip);
    }
    void StartWave()
    {


        PlayWaveVoiceLine(currentWave);

        waveCompleted = false;
        currentPhase = WavePhase.KillMonsters;

        WaveData wave = Waves[currentWave - 1];

        MaxEnemies = 0;

        foreach (WaveEnemy enemy in wave.Enemies)
        {
            MaxEnemies += enemy.Amount;
        }

        CurrentEnemies = MaxEnemies;



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
        {            return;
        }

        spawner.SpawnWave(wave.Enemies);
    }

    public void EnemyKilled()
    {
        if (waveCompleted)
            return;

        CurrentEnemies--;

        if (CurrentEnemies < 0)
            CurrentEnemies = 0;



        if (CurrentEnemies == 0)
        {
            WaveCompleted();
        }
    }

    public void WaveCompleted()
    {
        if (waveCompleted)
            return;

        waveCompleted = true;



        stats.AddEXP(300);

        if (currentWave >= 8)
        {

            UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
            return;
        }

        StartCoroutine(PreparePhase());
    }
    IEnumerator PreparePhase()
    {
        currentPhase =
            WavePhase.PrepareLoadout;

        CurrentPhaseTime =
            prepareTime;


        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayPrepare();

        while (CurrentPhaseTime > 0)
        {
            CurrentPhaseTime -= Time.deltaTime;

            yield return null;
        }

        NextWave();
    }

    void NextWave()
    {
        RunManager.Instance.WaveCompleted();

        WaveCompletedEvent?.Invoke();

        currentWave++;


        if (currentWave > Waves.Length)
        {            return;
        }

        StartWave();
    }
}