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
    }

    public void DisableLeftHand()
    {
        LeftHandHitBox.enabled = false;
    }

    public void EnableRightHand()
    {
        EnemiesHit.Clear();
        RightHandHitBox.enabled = true;
    }

    public void DisableRightHand()
    {
        RightHandHitBox.enabled = false;
    }

    public void DisableHands()
    {
        LeftHandHitBox.enabled = false;
        RightHandHitBox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyStatus enemy = other.GetComponentInParent<EnemyStatus>();

        if (enemy == null)
            return;

        if (EnemiesHit.Contains(enemy))
            return;

        EnemiesHit.Add(enemy);
        enemy.TakeDamage(Damage);
    }
}