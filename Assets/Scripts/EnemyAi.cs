using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public enum EnemyType
    {
        Slime,
        Wolf,
        Goblin,
        Skeleton
    }

    [Header("Enemy Type")]
    public EnemyType enemyType;

    [Header("References")]
    public NavMeshAgent agent;
    public Transform Player;
    public LayerMask WhatIsPlayer, WhatIsGround;
    public Animator animator;

    [Header("Patrolling")]
    public Vector3 WalkPoint;
    bool WalkPointSet;
    public float WalkRange;
    public float minIdleTime = 2f;
    public float maxIdleTime = 5f;
    private float idleTimer;
    private bool isWaiting;

    [Header("Attacking")]
    public float TimeBetweenAttacks;
    bool AlreadyAtacked;
    public int AttackDamage = 10;

    [Header("States")]
    public float SightRange, AttackRange;
    public bool PlayerInSightRange, PlayerInAttackRange;

    public void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        GameObject playerObj = GameObject.Find("PlayerObj");

        if (playerObj != null)
            Player = playerObj.transform;
    }

    void Update()
    {
        if (agent == null)
            return;

        CheckIfPlayerInRange();
    }

    public void CheckIfPlayerInRange()
    {
        PlayerInSightRange = Physics.CheckSphere(
            transform.position,
            SightRange,
            WhatIsPlayer
        );

        PlayerInAttackRange = Physics.CheckSphere(
            transform.position,
            AttackRange,
            WhatIsPlayer
        );

        if (!PlayerInSightRange && !PlayerInAttackRange)
            Patroling();

        if (PlayerInSightRange && !PlayerInAttackRange)
            Chaseing();

        if (PlayerInSightRange && PlayerInAttackRange)
            Attacking();
    }

    private void Patroling()
    {
        if (isWaiting)
        {
            idleTimer -= Time.deltaTime;

            if (animator != null)
                animator.SetTrigger("Idle");

            agent.SetDestination(transform.position);

            if (idleTimer <= 0f)
                isWaiting = false;

            return;
        }

        if (!WalkPointSet)
            GetWalkPoint();

        if (WalkPointSet)
        {
            agent.SetDestination(WalkPoint);

            if (animator != null)
                animator.SetTrigger("Patroling");
        }

        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            WalkPointSet = false;
            idleTimer = Random.Range(
                minIdleTime,
                maxIdleTime
            );
            isWaiting = true;
        }
    }

    private bool HaveChasingAnimation(EnemyType Type)
    {
        return Type == EnemyType.Wolf ||
               Type == EnemyType.Goblin ||
               Type == EnemyType.Skeleton;
    }

    private void Chaseing()
    {
        if (Player == null)
            return;

        agent.SetDestination(Player.position);

        if (HaveChasingAnimation(enemyType) &&
            animator != null)
        {
            animator.SetTrigger("Chasing");
        }
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position);

        if (AlreadyAtacked)
            return;

        FacePlayer();

        if (Player == null)
            return;

        PlayerStatus playerStatus =
            Player.GetComponentInParent<PlayerStatus>();

        if (playerStatus != null)
            playerStatus.TakeDamage(AttackDamage);

        AlreadyAtacked = true;

        Invoke(nameof(ResetAttack), TimeBetweenAttacks);

        if (animator != null)
            animator.SetTrigger("Attack");
    }
    public void AttackFinished()
    {
        AlreadyAtacked = false;
    }

    private void FacePlayer()
    {
        if (Player == null)
            return;

        Vector3 direction =
            Player.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        transform.rotation =
            Quaternion.LookRotation(
                direction.normalized
            );
    }

    private void ResetAttack()
    {
        AlreadyAtacked = false;
    }

    private void GetWalkPoint()
    {
        float randomZ =
            Random.Range(
                -WalkRange,
                WalkRange
            );

        float randomX =
            Random.Range(
                -WalkRange,
                WalkRange
            );

        Vector3 targetPoint =
            new Vector3(
                transform.position.x + randomX,
                transform.position.y + 10f,
                transform.position.z + randomZ
            );

        if (Physics.Raycast(
            targetPoint,
            Vector3.down,
            out RaycastHit hit,
            20f,
            WhatIsGround))
        {
            WalkPoint = hit.point;
            WalkPointSet = true;
        }
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
    }
}

