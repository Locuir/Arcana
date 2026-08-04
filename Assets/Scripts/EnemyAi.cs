using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform Player;
    public LayerMask WhatIsPlayer, WhatIsGround;

    [Header("Patrolling")]
    public Vector3 WalkPoint;
    bool WalkPointSet;
    public float WalkRange;

    [Header("Attacking")]
    public float TimeBetweenAttacks;
    bool AlreadyAtacked;

    [Header("States")]
    public float SightRange, AttackRange;
    public bool PlayerInSightRange, PlayerInAttackRange;


    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.Find("PlayerObj");
        if (playerObj != null)
        {
            Player = playerObj.transform;
        }



    }


    void Start()
    {
        Debug.Log(agent.isOnNavMesh);



    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerInRange();
    }


    public void CheckIfPlayerInRange()
    {
        PlayerInSightRange = Physics.CheckSphere(transform.position, SightRange, WhatIsPlayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, WhatIsPlayer);

        if (!PlayerInSightRange && !PlayerInAttackRange) Patroling();
        if (PlayerInSightRange && !PlayerInAttackRange) Chaseing();
        if (PlayerInSightRange && PlayerInAttackRange) Attacking();
    }


    private void Patroling()
    {
        if (!WalkPointSet) GetWalkPoint();

        if (WalkPointSet)
        {
            agent.SetDestination(WalkPoint);

            Debug.Log("Starting Patroling");
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            WalkPointSet = false;
        }

    }
    private void Chaseing()
    {

        agent.SetDestination(Player.position);
        
        Debug.Log("Starting Chaseing");

    }

    private void Attacking()
    {

        agent.SetDestination(transform.position);
        if (!AlreadyAtacked)
        {
            Player.GetComponentInParent<PlayerStatus>().TakeDamage(10);

            Debug.Log("Enemy Attacks!");

            AlreadyAtacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
        }


    

    }



    private void ResetAttack()
    {
        AlreadyAtacked = false;
    }

    private void GetWalkPoint()
    {
        float randomZ = Random.Range(-WalkRange, WalkRange);
        float randomX = Random.Range(-WalkRange, WalkRange);


        Vector3 targetPoint = new Vector3(transform.position.x + randomX, transform.position.y + 10f, transform.position.z + randomZ);

     
        if (Physics.Raycast(targetPoint, Vector3.down, out RaycastHit hit, 20f, WhatIsGround))
        {
            WalkPoint = hit.point;
            WalkPointSet = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

}
