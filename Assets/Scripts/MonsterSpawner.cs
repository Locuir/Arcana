using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] SpawnPoints;

    private int CurrentEnemies = 0;

    public void SpawnWave(WaveManager.WaveEnemy[] waveEnemies)
    {
        CurrentEnemies = 0;

        foreach (WaveManager.WaveEnemy enemyData in waveEnemies)
        {
            for (int i = 0; i < enemyData.Amount; i++)
            {
                SpawnEnemy(enemyData.EnemyPrefab);
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
            return;

        if (SpawnPoints == null || SpawnPoints.Length == 0)
        {
            Debug.LogError(
                "MONSTER SPAWNER → No Spawn Points assigned!"
            );
            return;
        }

        Transform spawnPoint =
            SpawnPoints[
                Random.Range(0, SpawnPoints.Length)
            ];

        if (spawnPoint == null)
            return;

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        EnemyStatus enemyStatus =
            enemy.GetComponent<EnemyStatus>();

        if (enemyStatus != null)
            enemyStatus.Spawner = this;

        CurrentEnemies++;
    }

    public void EnemyDied()
    {
        CurrentEnemies--;

        if (CurrentEnemies <= 0)
        {
            CurrentEnemies = 0;

            if (WaveManager.Instance != null)
                WaveManager.Instance.EnemyKilled();
        }
    }
}