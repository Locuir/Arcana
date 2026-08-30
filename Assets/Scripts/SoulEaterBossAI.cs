using UnityEngine;
using UnityEngine.AI;

public class SoulEaterBossAI : MonoBehaviour
{
    public enum BossState
    {
        Sleeping,
        Intro,
        GroundCombat,
        TakingOff,
        Flying,
        Landing,
        Dead
    }

    [Header("Boss Audio")]
    public AudioSource AudioSource;
    public AudioClip FlyingSound;
    public AudioClip ScreamSound;
    public AudioClip FireballSound;
    public AudioClip GetHitSound;
    public AudioClip BiteSound;
    public AudioClip StepSound;

    [Header("Hit Reaction")]
    public float GetHitCooldown = 0.5f;
    private float lastHitTime = -Mathf.Infinity;

    [Header("Ground Protection")]
    public float GroundOffset = 1f;

    [Header("References")]
    public Transform Player;
    public Animator animator;
    public NavMeshAgent agent;
    public EnemyStatus enemyStatus;
    public LayerMask WhatIsPlayer, WhatIsGround;

    [Header("Detection")]
    public float SightRange = 30f;
    public float AttackRange = 7f;

    [Header("Ground Combat")]
    public float BasicAttackRange = 4f;
    public float TailAttackRange = 7f;
    public float FireballRange = 20f;
    public int BasicAttackDamage = 20;
    public int TailAttackDamage = 30;

    [Header("Attack Delays")]
    public float BasicAttackDelay = 1.5f;
    public float TailAttackDelay = 2f;
    public float FireballDelay = 2f;
    public float DefendDelay = 3f;

    [Header("Combo")]
    public int MaxBasicCombo = 2;
    public float ComboResetTime = 3f;

    [Header("Flying")]
    public float FlyingHeight = 10f;
    public float FlyingSpeed = 8f;
    public float FlyingDuration = 12f;
    public float FlyAttackDelay = 4f;

    [Header("Flying Position")]
    public float FlyingHorizontalDistance = 7f;
    public float FlyingSideOffset = 3f;

    [Header("Phase 2")]
    [Range(0f, 1f)]
    public float FlyingPhaseHealth = 0.5f;

    [Header("Boss State")]
    public BossState currentState = BossState.Sleeping;

    [Header("Fireball")]
    public GameObject FireballPrefab;
    public Transform FireballSpawnPoint;

    private bool introFinished;
    private bool canAttack = true;
    private bool phaseTwoStarted;
    private float flyingTimer;

    private float flyingY;
    private Vector3 flyingTargetPosition;

    private int basicComboCount;
    private float lastAttackTime;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (enemyStatus == null)
            enemyStatus = GetComponent<EnemyStatus>();

        FindPlayer();
    }

    private void Start()
    {
        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.stoppingDistance = AttackRange;

        animator.SetFloat("Speed", 0f);
        animator.SetBool("IsFlying", false);
    }

    private void Update()
    {
        if (currentState == BossState.Dead)
            return;

        if (Player == null)
        {
            FindPlayer();
            return;
        }

        CheckDeath();

        if (currentState == BossState.Dead)
            return;

        if (Time.time - lastAttackTime > ComboResetTime)
            basicComboCount = 0;

        switch (currentState)
        {
            case BossState.Sleeping:
                HandleSleeping();
                break;

            case BossState.Intro:
                HandleIntro();
                break;

            case BossState.GroundCombat:
                HandleGroundCombat();
                break;

            case BossState.TakingOff:
                HandleTakingOff();
                break;

            case BossState.Flying:
                HandleFlying();
                break;

            case BossState.Landing:
                HandleLanding();
                break;
        }
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.Find("PlayerObj");

        if (playerObj != null)
            Player = playerObj.transform;
    }

    private void HandleSleeping()
    {
        StopAgent();

        FacePlayer();

        float distance =
            Vector3.Distance(
                transform.position,
                Player.position
            );

        if (distance <= SightRange)
            StartIntro();
    }

    private void StartIntro()
    {
        currentState = BossState.Intro;

        StopAgent();

        animator.SetTrigger("Scream");
    }

    public void FinishIntro()
    {
        if (currentState != BossState.Intro)
            return;

        introFinished = true;
        currentState = BossState.GroundCombat;

        ResumeAgent();
    }

    private void HandleIntro()
    {
        FacePlayer();
    }

    private void HandleGroundCombat()
    {
        if (!introFinished)
            return;

        if (agent == null ||
            !agent.enabled ||
            !agent.isOnNavMesh)
            return;

        CheckFlyingPhase();

        if (currentState != BossState.GroundCombat)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                Player.position
            );

        if (distance > AttackRange)
        {
            ChasePlayer();
            return;
        }

        StopAgent();

        animator.SetFloat("Speed", 0f);

        FacePlayer();

        if (canAttack)
            ChooseGroundAttack(distance);
    }

    private void ChooseGroundAttack(float distance)
    {
        bool playerBehind = IsPlayerBehind();
        bool playerClose = distance <= BasicAttackRange;

        if (playerBehind)
        {
            TailAttack();
            return;
        }

        if (playerClose)
        {
            if (basicComboCount < MaxBasicCombo)
            {
                float roll = Random.value;

                if (roll < 0.65f)
                {
                    BasicAttack();
                    return;
                }

                if (roll < 0.9f)
                {
                    TailAttack();
                    return;
                }

                Defend();
                return;
            }

            float finisherRoll = Random.value;

            if (finisherRoll < 0.5f)
            {
                TailAttack();
                return;
            }

            Defend();
            return;
        }

        if (distance <= TailAttackRange)
        {
            float roll = Random.value;

            if (roll < 0.55f)
            {
                TailAttack();
                return;
            }

            Fireball();
            return;
        }

        if (distance <= FireballRange)
        {
            Fireball();
        }
    }

    private bool IsPlayerBehind()
    {
        Vector3 directionToPlayer =
            (Player.position - transform.position).normalized;

        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude < 0.01f)
            return false;

        float angle =
            Vector3.Angle(
                transform.forward,
                directionToPlayer
            );

        return angle > 120f;
    }

    private void BasicAttack()
    {
        StartAttack(BasicAttackDelay);

        basicComboCount++;
        lastAttackTime = Time.time;

        FacePlayer();

        animator.SetTrigger("BasicAttack");
    }

    private void TailAttack()
    {
        StartAttack(TailAttackDelay);

        basicComboCount = 0;
        lastAttackTime = Time.time;

        FacePlayer();

        animator.SetTrigger("TailAttack");
    }

    private void Fireball()
    {
        StartAttack(FireballDelay);

        basicComboCount = 0;
        lastAttackTime = Time.time;

        FacePlayer();

        animator.SetTrigger("Fireball");
    }

    private void Defend()
    {
        StartAttack(DefendDelay);

        basicComboCount = 0;
        lastAttackTime = Time.time;

        FacePlayer();

        animator.SetTrigger("Defend");
    }

    private void FlyAttack()
    {
        StartAttack(FlyAttackDelay);

        FacePlayer();

        animator.SetTrigger("FlyAttack");
    }

    private void StartAttack(float delay)
    {
        canAttack = false;

        CancelInvoke(nameof(ResetAttack));

        Invoke(
            nameof(ResetAttack),
            delay
        );
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    private void CheckFlyingPhase()
    {
        if (phaseTwoStarted)
            return;

        if (enemyStatus == null)
            return;

        float healthPercent =
            (float)enemyStatus.Health /
            enemyStatus.MaxHealth;

        if (healthPercent <= FlyingPhaseHealth)
            StartFlyingPhase();
    }

    private void StartFlyingPhase()
    {
        phaseTwoStarted = true;

        currentState =
            BossState.TakingOff;

        StopAgent();

        flyingY =
            transform.position.y +
            FlyingHeight;

        animator.SetFloat("Speed", 0f);

        animator.SetTrigger("TakeOff");
    }

    public void StartFlying()
    {
        if (currentState != BossState.TakingOff)
            return;

        currentState =
            BossState.Flying;

        animator.SetBool(
            "IsFlying",
            true
        );

        StartFlyingSound();

        flyingTimer =
            FlyingDuration;

        flyingY =
            transform.position.y +
            FlyingHeight;

        float frontBack =
            Random.value > 0.5f
                ? 1f
                : -1f;

        float sideOffset =
            Random.Range(
                -FlyingSideOffset,
                FlyingSideOffset
            );

        Vector3 horizontalOffset =
            Player.forward *
            FlyingHorizontalDistance *
            frontBack;

        horizontalOffset +=
            Player.right *
            sideOffset;

        flyingTargetPosition =
            Player.position +
            horizontalOffset;

        flyingTargetPosition.y =
            flyingY;

        if (agent != null &&
            agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
    }

    private void HandleTakingOff()
    {
        FacePlayer();
    }

    private void HandleFlying()
    {
        if (Player == null)
            return;

        flyingTimer -= Time.deltaTime;

        Vector3 targetPosition =
            flyingTargetPosition;

        Vector3 direction =
            targetPosition -
            transform.position;

        if (direction.sqrMagnitude > 0.1f)
        {
            transform.position +=
                direction.normalized *
                FlyingSpeed *
                Time.deltaTime;
        }

        if (Terrain.activeTerrain != null)
        {
            float groundY =
                Terrain.activeTerrain.SampleHeight(
                    transform.position
                ) +
                Terrain.activeTerrain.transform.position.y;

            float minimumY =
                groundY +
                GroundOffset;

            if (transform.position.y < minimumY)
            {
                Vector3 position =
                    transform.position;

                position.y =
                    minimumY;

                transform.position =
                    position;
            }
        }

        FacePlayer();

        float distance =
            Vector3.Distance(
                transform.position,
                Player.position
            );

        animator.SetFloat(
            "Speed",
            distance > 2f ? 1f : 0f
        );

        if (canAttack &&
            distance <= FireballRange)
        {
            FlyAttack();
        }

        if (flyingTimer <= 0f)
            StartLanding();
    }

    private void HandleLanding()
    {
        FacePlayer();

        animator.SetFloat("Speed", 0f);
    }

    private void ChasePlayer()
    {
        if (agent == null)
            return;

        if (!agent.enabled)
            return;

        if (!agent.isOnNavMesh)
            return;

        if (Player == null)
            return;

        agent.isStopped = false;

        agent.SetDestination(
            Player.position
        );

        float speed =
            agent.velocity.magnitude;

        animator.SetFloat(
            "Speed",
            speed > 0.1f ? 1f : 0f
        );
    }

    private void StartLanding()
    {
        if (currentState != BossState.Flying)
            return;

        currentState =
            BossState.Landing;

        animator.SetFloat(
            "Speed",
            0f
        );

        animator.SetBool(
            "IsFlying",
            false
        );

        animator.SetTrigger(
            "Land"
        );

        StopFlyingSound();
    }

    public void FinishLanding()
    {
        if (currentState != BossState.Landing)
            return;

        if (Player == null)
        {
            Debug.LogError(
                "SoulEater: Player is missing."
            );

            return;
        }

        Vector3 targetPosition =
            Player.position;

        NavMeshHit navHit;

        if (!NavMesh.SamplePosition(
            targetPosition,
            out navHit,
            20f,
            NavMesh.AllAreas))
        {
            Debug.LogError(
                "SoulEater: Could not find NavMesh near Player."
            );

            return;
        }

        transform.position =
            navHit.position;

        if (agent != null)
        {
            agent.enabled = true;

            if (!agent.isOnNavMesh)
            {
                if (!agent.Warp(
                    navHit.position))
                {
                    Debug.LogError(
                        "SoulEater: Failed to Warp onto NavMesh."
                    );

                    return;
                }
            }

            agent.isStopped = false;
            agent.ResetPath();
        }

        currentState =
            BossState.GroundCombat;

        animator.SetFloat(
            "Speed",
            0f
        );
    }

    public void GetHit()
    {
        if (currentState == BossState.Dead)
            return;

        if (currentState == BossState.Sleeping ||
            currentState == BossState.Intro)
            return;

        if (Time.time <
            lastHitTime +
            GetHitCooldown)
            return;

        lastHitTime =
            Time.time;

        animator.SetTrigger(
            "GetHit"
        );
    }

    private void CheckDeath()
    {
        if (enemyStatus == null)
            return;

        if (enemyStatus.Health <= 0)
            Die();
    }

    public void Die()
    {
        if (currentState == BossState.Dead)
            return;

        currentState =
            BossState.Dead;

        CancelInvoke();

        StopFlyingSound();

        if (agent != null &&
            agent.enabled &&
            agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        animator.SetFloat(
            "Speed",
            0f
        );

        animator.SetBool(
            "IsFlying",
            false
        );

        animator.SetTrigger(
            "Die"
        );
    }

    private void StopAgent()
    {
        if (agent == null)
            return;

        if (!agent.enabled)
            return;

        if (!agent.isOnNavMesh)
            return;

        agent.isStopped = true;
        agent.ResetPath();
    }

    private bool ResumeAgent()
    {
        if (agent == null)
            return false;

        if (!agent.enabled)
            return false;

        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(
                transform.position,
                out hit,
                5f,
                NavMesh.AllAreas))
            {
                agent.Warp(
                    hit.position
                );
            }
            else
            {
                Debug.LogWarning(
                    "SoulEater is not on NavMesh."
                );

                return false;
            }
        }

        agent.isStopped = false;

        return true;
    }

    public void FinishDeath()
    {
        if (currentState != BossState.Dead)
            return;

        Destroy(gameObject);
    }

    public void FireDragonFireball()
    {
        if (FireballPrefab == null)
        {
            Debug.LogError(
                "FireballPrefab is not assigned!"
            );

            return;
        }

        if (FireballSpawnPoint == null)
        {
            Debug.LogError(
                "FireballSpawnPoint is not assigned!"
            );

            return;
        }

        if (Player == null)
        {
            Debug.LogError(
                "Player is not assigned!"
            );

            return;
        }

        GameObject fireball =
            Instantiate(
                FireballPrefab,
                FireballSpawnPoint.position,
                FireballSpawnPoint.rotation
            );

        DragonFireball projectile =
            fireball.GetComponent<DragonFireball>();

        if (projectile == null)
        {
            Debug.LogError(
                "DragonFireball component is missing from FireballPrefab!"
            );

            Destroy(fireball);

            return;
        }

        projectile.Initialize(
            Player.position
        );
    }

    public void BasicAttackDamageEvent()
    {
        if (Player == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                Player.position
            );

        if (distance <= BasicAttackRange)
        {
            PlayerStatus playerStatus =
                Player.GetComponentInParent<PlayerStatus>();

            if (playerStatus != null)
                playerStatus.TakeDamage(
                    BasicAttackDamage
                );
        }
    }

    public void TailAttackDamageEvent()
    {
        if (Player == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                Player.position
            );

        if (distance <= TailAttackRange)
        {
            PlayerStatus playerStatus =
                Player.GetComponentInParent<PlayerStatus>();

            if (playerStatus != null)
                playerStatus.TakeDamage(
                    TailAttackDamage
                );
        }
    }

    public void PlayScreamSound()
    {
        if (AudioSource != null &&
            ScreamSound != null)
            AudioSource.PlayOneShot(
                ScreamSound
            );
    }

    public void PlayBiteSound()
    {
        if (AudioSource != null &&
            BiteSound != null)
            AudioSource.PlayOneShot(
                BiteSound
            );
    }

    public void PlayStepSound()
    {
        if (AudioSource != null &&
            StepSound != null)
            AudioSource.PlayOneShot(
                StepSound
            );
    }

    public void PlayFireballSound()
    {
        if (AudioSource != null &&
            FireballSound != null)
            AudioSource.PlayOneShot(
                FireballSound
            );
    }

    public void PlayGetHitSound()
    {
        if (AudioSource != null &&
            GetHitSound != null)
            AudioSource.PlayOneShot(
                GetHitSound
            );
    }

    private void StartFlyingSound()
    {
        if (AudioSource == null ||
            FlyingSound == null)
            return;

        if (AudioSource.clip == FlyingSound &&
            AudioSource.isPlaying)
            return;

        AudioSource.clip =
            FlyingSound;

        AudioSource.loop = true;
        AudioSource.Play();
    }

    private void StopFlyingSound()
    {
        if (AudioSource == null)
            return;

        if (AudioSource.clip == FlyingSound)
        {
            AudioSource.Stop();
            AudioSource.clip = null;
            AudioSource.loop = false;
        }
    }

    private void FacePlayer()
    {
        if (Player == null)
            return;

        Vector3 direction =
            Player.position -
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
                8f *
                Time.deltaTime
            );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            SightRange
        );

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            AttackRange
        );

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(
            transform.position,
            BasicAttackRange
        );

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            transform.position,
            TailAttackRange
        );

        Gizmos.color = Color.magenta;

        Gizmos.DrawWireSphere(
            transform.position,
            FireballRange
        );
    }
}

