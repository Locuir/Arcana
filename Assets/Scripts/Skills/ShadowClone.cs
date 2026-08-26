using UnityEngine;
using UnityEngine.AI;

public class ShadowClone : MonoBehaviour
{
    [Header("Skill")]
    public SkillData skillData;
    public float Duration = 8f;

    [Header("Combat")]
    public float DamageMultiplier = 0.5f;
    public float AttackRange = 2f;
    public float TimeBetweenAttacks = 1f;
    public float MoveSpeed = 4f;

    [Header("References")]
    public PlayerStats playerStats;
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Effect")]
    public ParticleSystem SpawnParticle;

    private EnemyStatus currentTarget;
    private float attackTimer;
    private float cooldownTimer;
    private bool isActive;

    private void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (playerStats == null)
            playerStats = FindFirstObjectByType<PlayerStats>();

        if (agent != null)
            agent.speed = MoveSpeed;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isActive)
            return;

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        attackTimer -= Time.deltaTime;

        FindTarget();

        if (currentTarget == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                currentTarget.transform.position
            );

        if (distance > AttackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(
                currentTarget.transform.position
            );

            if (animator != null)
                animator.SetTrigger("Chasing");
        }
        else
        {
            agent.isStopped = true;

            FaceTarget();

            if (attackTimer <= 0f)
                Attack();
        }
    }

    public void Activate()
    {
        Debug.Log("SHADOW CLONE → Activate called");

        if (skillData == null)
        {
            Debug.LogError(
                "SHADOW CLONE → SkillData is NULL!"
            );
            return;
        }

        if (!skillData.unlocked)
        {
            Debug.Log(
                "SHADOW CLONE → Skill is LOCKED!"
            );
            return;
        }

        if (isActive)
        {
            Debug.Log(
                "SHADOW CLONE → Already active!"
            );
            return;
        }

        if (cooldownTimer > 0f)
        {
            Debug.Log(
                "SHADOW CLONE → On cooldown: " +
                cooldownTimer
            );
            return;
        }

        if (playerStats == null)
        {
            Debug.LogError(
                "SHADOW CLONE → PlayerStats is NULL!"
            );
            return;
        }

        Vector3 spawnPosition =
            transform.parent != null
                ? transform.parent.position
                : transform.position;

        transform.position =
            spawnPosition +
            transform.parent.right * 1.5f;

        gameObject.SetActive(true);

        isActive = true;
        cooldownTimer = skillData.cooldown;

        if (SpawnParticle != null)
            SpawnParticle.Play();

        Debug.Log(
            "SHADOW CLONE → ACTIVATED | Duration: " +
            Duration +
            " | Damage: " +
            (DamageMultiplier * 100f) +
            "%"
        );

        CancelInvoke(nameof(Deactivate));
        Invoke(nameof(Deactivate), Duration);
    }

    private void FindTarget()
    {
        EnemyAi[] enemies =
            FindObjectsByType<EnemyAi>(
                FindObjectsSortMode.None
            );

        float closestDistance =
            Mathf.Infinity;

        EnemyStatus closestEnemy = null;

        foreach (EnemyAi enemy in enemies)
        {
            if (enemy == null)
                continue;

            EnemyStatus status =
                enemy.GetComponent<EnemyStatus>();

            if (status == null)
                continue;

            float distance =
                Vector3.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = status;
            }
        }

        currentTarget = closestEnemy;
    }

    private void Attack()
    {
        if (currentTarget == null)
            return;

        if (playerStats == null)
            return;

        int damage =
            Mathf.RoundToInt(
                playerStats.AttackPower *
                DamageMultiplier
            );

        if (damage < 1)
            damage = 1;

        currentTarget.TakeDamage(damage);

        attackTimer = TimeBetweenAttacks;

        Debug.Log(
            "SHADOW CLONE → ATTACK | " +
            currentTarget.name +
            " | Damage: " +
            damage
        );

        if (animator != null)
            animator.SetTrigger("Attack");
    }

    private void FaceTarget()
    {
        if (currentTarget == null)
            return;

        Vector3 direction =
            currentTarget.transform.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        transform.rotation =
            Quaternion.LookRotation(
                direction.normalized
            );
    }

    private void Deactivate()
    {
        Debug.Log("SHADOW CLONE → ENDED");

        isActive = false;
        currentTarget = null;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        gameObject.SetActive(false);
    }

    public bool IsSkillReady()
    {
        return !isActive && cooldownTimer <= 0f;
    }

    public bool IsSkillActive()
    {
        return isActive;
    }

    public float GetCooldownPercent()
    {
        if (skillData == null)
            return 0f;

        if (skillData.cooldown <= 0f)
            return 0f;

        return Mathf.Clamp01(
            cooldownTimer / skillData.cooldown
        );
    }
}