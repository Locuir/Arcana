using UnityEngine;
using UnityEngine.AI;

public class ShadowMinion : MonoBehaviour
{
    [Header("Minion")]
    public float Duration = 10f;
    public float DamageMultiplier = 0.5f;
    public float DeathAnimationDuration = 2f;

    [Header("AI")]
    public float SightRange = 15f;
    public float AttackRange = 2f;
    public float TimeBetweenAttacks = 1.5f;

    [Header("Patrolling")]
    public float PatrolRange = 5f;
    public float PatrolWaitTime = 2f;

    [Header("References")]
    public NavMeshAgent agent;
    public Animator animator;
    public PlayerStats playerStats;

    private EnemyStatus target;
    private float attackTimer;
    private float lifeTimer;
    private float patrolWaitTimer;

    private Vector3 spawnPosition;
    private Vector3 patrolPoint;

    private bool patrolPointSet;
    private bool isDying;

    private void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (playerStats == null)
            playerStats = FindFirstObjectByType<PlayerStats>();

        spawnPosition = transform.position;

        lifeTimer = Duration;

        GetPatrolPoint();

        Debug.Log("SHADOW MINION → SPAWNED");
    }

    private void Update()
    {
        if (isDying)
            return;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            Die();
            return;
        }

        attackTimer -= Time.deltaTime;

        FindTarget();

        if (target == null)
        {
            Patroling();
            return;
        }

        float distance =
            Vector3.Distance(
                transform.position,
                target.transform.position
            );

        if (distance > AttackRange)
        {
            Chase();
        }
        else
        {
            Attack();
        }
    }

    private void FindTarget()
    {
        EnemyStatus[] enemies =
            FindObjectsByType<EnemyStatus>(
                FindObjectsSortMode.None
            );

        float closestDistance = SightRange;
        EnemyStatus closestEnemy = null;

        foreach (EnemyStatus enemy in enemies)
        {
            if (enemy == null)
                continue;

            float distance =
                Vector3.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        target = closestEnemy;
    }

    private void Patroling()
    {
        if (agent == null)
            return;

        if (!patrolPointSet)
        {
            GetPatrolPoint();
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(patrolPoint);

        if (animator != null)
            animator.SetTrigger("Petrolling");

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolPointSet = false;

            patrolWaitTimer -= Time.deltaTime;

            if (patrolWaitTimer <= 0f)
            {
                patrolWaitTimer = PatrolWaitTime;
                GetPatrolPoint();
            }
        }
    }

    private void GetPatrolPoint()
    {
        float randomX =
            Random.Range(
                -PatrolRange,
                PatrolRange
            );

        float randomZ =
            Random.Range(
                -PatrolRange,
                PatrolRange
            );

        Vector3 randomPoint =
            spawnPosition +
            new Vector3(
                randomX,
                0f,
                randomZ
            );

        if (NavMesh.SamplePosition(
            randomPoint,
            out NavMeshHit hit,
            PatrolRange,
            NavMesh.AllAreas))
        {
            patrolPoint = hit.position;
            patrolPointSet = true;
        }
    }

    private void Chase()
    {
        if (target == null)
            return;

        if (agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(
                target.transform.position
            );
        }

        if (animator != null)
            animator.SetTrigger("Chaseing");
    }

    private void Attack()
    {
        if (target == null)
            return;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        FaceTarget();

        if (attackTimer > 0f)
            return;

        int damage = 1;

        if (playerStats != null)
        {
            damage =
                Mathf.RoundToInt(
                    playerStats.AttackPower *
                    DamageMultiplier
                );
        }

        if (damage < 1)
            damage = 1;

        target.TakeDamage(damage);

        attackTimer = TimeBetweenAttacks;

        if (animator != null)
            animator.SetTrigger("Attack");

        Debug.Log(
            "SHADOW MINION → ATTACK | Damage: " +
            damage
        );
    }

    private void FaceTarget()
    {
        if (target == null)
            return;

        Vector3 direction =
            target.transform.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(
                direction.normalized
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
    }

    private void Die()
    {
        if (isDying)
            return;

        isDying = true;

        target = null;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (animator != null)
            animator.SetTrigger("Die");

        Debug.Log("SHADOW MINION → DIE");

        Destroy(
            gameObject,
            DeathAnimationDuration
        );
    }
}