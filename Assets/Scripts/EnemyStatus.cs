using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public enum EnemyType { Slime, Wolf, Goblin, Skeleton }

    [Header("Enemy Type")]
    public EnemyType enemyType;
    public EnemyAi enemyAi;

    [Header("Death Sound")]
    public AudioClip DeathSound;
    public AudioSource AudioSource;


    public int Health = 30;
    public int MaxHealth = 30;

    bool IsDead = false;
    public MonsterSpawner Spawner;
    public GameObject CardPrefap;
    public float DeathDelay = 1;
    public Animator animator;
    public ParticleSystem DeathParticles;

    bool SkeletonRevived = false;

    public void TakeDamage(int DamageTaken)
    {
        Health -= DamageTaken;

        if (enemyType == EnemyType.Skeleton)
        {
            animator.SetTrigger("TakeDamage");
        }

        if (Health < 0)
            Health = 0;

        Debug.Log($"Damage Taken: {DamageTaken}");
        Debug.Log($"Health = {Health}");

        CheckDeath();
    }

    private bool HaveDeathAnimation(EnemyType Type)
    {
        return Type == EnemyType.Wolf ||
               Type == EnemyType.Goblin ||
               Type == EnemyType.Skeleton;
    }

    void CheckDeath()
    {
        if (IsDead) return;

        if (Health <= 0)
        {
            IsDead = true;
            Debug.Log("Dead");
            if (DeathSound != null && AudioSource != null)
                AudioSource.PlayOneShot(DeathSound);
            if (WaveManager.Instance != null)
            {
                WaveManager.Instance.EnemyKilled();
            }

            if (HaveDeathAnimation(enemyType))
            {
                animator.SetTrigger("Death");


                DeathParticles.transform.SetParent(null);
                DeathParticles.Play();

                if (enemyType == EnemyType.Skeleton && SkeletonRevived == false)
                {
                    enemyAi.enabled = false;
                    SkeletonRevived = true;

                    Invoke(nameof(TriggerRevive), 1.5f);

                    return;
                }

                Destroy(DeathParticles.gameObject, 4f);
                Invoke(nameof(DestroyEnemy), DeathDelay);
            }
            else if (enemyType == EnemyType.Slime)
            {
                DeathParticles.transform.SetParent(null);
                DeathParticles.Play();

                Destroy(DeathParticles.gameObject, 2f);
                Invoke(nameof(DestroyEnemy), DeathDelay);
            }

            Instantiate(
                CardPrefap,
                transform.position + Vector3.up * 0.5f,
                Quaternion.identity
            );

            Spawner.EnemyDied();
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void TriggerRevive()
    {
        animator.SetTrigger("Revive");

        IsDead = false;
        Health = MaxHealth;

        Invoke(nameof(EnableAI), 5f);
    }

    void EnableAI()
    {
        if (enemyAi != null)
            enemyAi.enabled = true;
    }
}