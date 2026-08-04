using UnityEngine;

public class EnemyStatus : MonoBehaviour
{

    public int Health = 30;
    public int MaxHealth = 30;
    bool IsDead = false;
    public MonsterSpawner Spawner;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeDamage(int DamageTaken)
    {
        Health -= DamageTaken;

        if (Health < 0)
            Health = 0;

        Debug.Log($"Damage Taken: {DamageTaken}");
        Debug.Log($"Health = {Health}");
        CheckDeath();

    }

    void CheckDeath()
    {
        if (IsDead) return;

        if (Health <= 0)
        {
            IsDead = true;
            Debug.Log("Dead");

            Spawner.EnemyDied();
            Destroy(gameObject);
        }
    }

}
