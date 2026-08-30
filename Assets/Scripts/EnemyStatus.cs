using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public enum EnemyType
    {
        Slime,
        Wolf,
        Goblin,
        Skeleton,
        SoulEater
    }
    PlayerStats stats;

    [Header("Enemy Type")]
    public EnemyType enemyType;

    [Header("References")]
    public EnemyAi enemyAi;
    public SoulEaterBossAI bossAI;

    [Header("Health")]
    public int Health = 30;
    public int MaxHealth = 30;

    [Header("Death")]
    public AudioClip DeathSound;
    public AudioSource AudioSource;
    public Animator animator;
    public ParticleSystem DeathParticles;
    public MonsterSpawner Spawner;
    public GameObject CardPrefap;
    public float DeathDelay = 1f;

    private bool IsDead = false;
    private bool SkeletonRevived = false;

    public void TakeDamage(int DamageTaken)
    {
        if (IsDead)
            return;

        Health -= DamageTaken;

        if (Health < 0)
            Health = 0;

        Debug.Log($"Damage Taken: {DamageTaken}");
        Debug.Log($"Health = {Health}");

        if (enemyType == EnemyType.SoulEater)
        {
            if (Health <= 0)
            {
                IsDead = true;

                if (DeathSound != null && AudioSource != null)
                    AudioSource.PlayOneShot(DeathSound);

                if (WaveManager.Instance != null)
                    WaveManager.Instance.EnemyKilled();

                if (bossAI != null)
                    DeathParticles.Play();
                    stats.AddEXP(350);
                    bossAI.Die();
            }
            else
            {
                if (bossAI != null)
                    bossAI.GetHit();
            }

            return;
        }

        if (enemyType == EnemyType.Skeleton)
        {
            animator.SetTrigger("TakeDamage");
        }

        CheckDeath();
    }

    private bool HaveDeathAnimation(EnemyType Type)
    {
        return Type == EnemyType.Wolf ||
               Type == EnemyType.Goblin ||
               Type == EnemyType.Skeleton;
    }

    private void CheckDeath()
    {
        if (IsDead)
            return;

        if (Health <= 0)
        {
            IsDead = true;

            Debug.Log("Dead");

            if (DeathSound != null && AudioSource != null)
                AudioSource.PlayOneShot(DeathSound);

            if (WaveManager.Instance != null)
                WaveManager.Instance.EnemyKilled();

            if (HaveDeathAnimation(enemyType))
            {
                animator.SetTrigger("Death");

                if (DeathParticles != null)
                {
                    DeathParticles.transform.SetParent(null);
                    DeathParticles.Play();

                    Destroy(DeathParticles.gameObject, 4f);
                }

                if (enemyType == EnemyType.Skeleton && !SkeletonRevived)
                {
                    if (enemyAi != null)
                        enemyAi.enabled = false;

                    SkeletonRevived = true;

                    Invoke(nameof(TriggerRevive), 1.5f);

                    return;
                }

                Invoke(nameof(DestroyEnemy), DeathDelay);
            }
            else if (enemyType == EnemyType.Slime)
            {
                if (DeathParticles != null)
                {
                    DeathParticles.transform.SetParent(null);
                    DeathParticles.Play();

                    Destroy(DeathParticles.gameObject, 2f);
                }

                Invoke(nameof(DestroyEnemy), DeathDelay);
            }

            if (CardPrefap != null)
            {
                Instantiate(
                    CardPrefap,
                    transform.position + Vector3.up * 0.5f,
                    Quaternion.identity
                );
            }

            if (Spawner != null)
                Spawner.EnemyDied();
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void TriggerRevive()
    {
        animator.SetTrigger("Revive");

        IsDead = false;
        Health = MaxHealth;

        Invoke(nameof(EnableAI), 5f);
    }

    private void EnableAI()
    {
        if (enemyAi != null)
            enemyAi.enabled = true;
    }
}