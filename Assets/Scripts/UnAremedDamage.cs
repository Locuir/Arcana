
using System.Collections.Generic;
using UnityEngine;

public class UnArmedDamage : MonoBehaviour
{
    public Collider LeftHandHitBox;
    public Collider RightHandHitBox;

    public int Damage = 10;

    private HashSet<EnemyStatus> EnemiesHit = new HashSet<EnemyStatus>();

    private void Start()
    {
        LeftHandHitBox.enabled = false;
        RightHandHitBox.enabled = false;
    }

    public void EnableLeftHand()
    {
        EnemiesHit.Clear();
        LeftHandHitBox.enabled = true;
        Debug.Log("Left Hand Enabled");
    }

    public void DisableLeftHand()
    {
        LeftHandHitBox.enabled = false;
        Debug.Log("Left Hand Disabled");
    }

    public void EnableRightHand()
    {
        EnemiesHit.Clear();
        RightHandHitBox.enabled = true;
        Debug.Log("Right Hand Enabled");
    }

    public void DisableRightHand()
    {
        RightHandHitBox.enabled = false;
        Debug.Log("Right Hand Disabled");
    }

    public void DisableHands()
    {
        LeftHandHitBox.enabled = false;
        RightHandHitBox.enabled = false;
        EnemiesHit.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDealDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!LeftHandHitBox.enabled && !RightHandHitBox.enabled)
            return;

        TryDealDamage(other);
    }

    private void TryDealDamage(Collider other)
    {
        EnemyStatus enemy = other.GetComponentInParent<EnemyStatus>();

        if (enemy == null)
            return;

        if (EnemiesHit.Contains(enemy))
        {
            Debug.Log("Enemy already hit this attack");
            return;
        }

        EnemiesHit.Add(enemy);

        Debug.Log("UNARMED HIT → " + enemy.name + " | Damage = " + Damage);

        enemy.TakeDamage(Damage);

        Debug.Log("Enemy Health After Hit = " + enemy.Health);
    }
}

