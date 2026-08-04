using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{

    public GameObject EnemyPrefab;

    public int MaxEnemies = 5;
    public float RespawnTime = 10f;

    private int CurrentEnemies = 0;

    void Start()
    {
        for (int i = 0; i < MaxEnemies; i++)
        {
            SpawnEnemy();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);

        enemy.GetComponent<EnemyStatus>().Spawner = this;

        CurrentEnemies++;
    }

    public void EnemyDied()
    {
        CurrentEnemies--;

        StartCoroutine(RespawnEnemy());
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(RespawnTime);

        if (CurrentEnemies < MaxEnemies)
        {
            SpawnEnemy();
        }
    }
}
