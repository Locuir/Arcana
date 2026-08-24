using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject EnemyPrefab;
        public int Amount;
    }

    public EnemySpawnData[] EnemyTypes;

    private int CurrentEnemies = 0;

    public void SpawnWave(EnemySpawnData[] waveEnemies)
    {
        CurrentEnemies = 0;

        foreach (EnemySpawnData enemyData in waveEnemies)
        {
            for (int i = 0; i < enemyData.Amount; i++)
            {
                SpawnEnemy(enemyData.EnemyPrefab);
            }
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(
            enemyPrefab,
            transform.position,
            Quaternion.identity
        );

        enemy.GetComponent<EnemyStatus>().Spawner = this;

        CurrentEnemies++;
    }

    public void EnemyDied()
    {
        CurrentEnemies--;

        if (CurrentEnemies <= 0)
        {
            CurrentEnemies = 0;
            WaveManager.Instance.EnemyKilled();
        }
    }
}