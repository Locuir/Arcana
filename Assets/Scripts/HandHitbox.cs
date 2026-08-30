using System.Collections.Generic;
using UnityEngine;

public class HandHitbox : MonoBehaviour
{
    public int Damage = 10;

    private HashSet<EnemyStatus> EnemiesHit = new HashSet<EnemyStatus>();

    public void ResetHits()
    {
        EnemiesHit.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HAND TRIGGER: " + other.name);

        EnemyStatus enemy = other.GetComponentInParent<EnemyStatus>();

        if (enemy == null)
            return;

        if (EnemiesHit.Contains(enemy))
            return;

        EnemiesHit.Add(enemy);

        enemy.TakeDamage(Damage);

        Debug.Log("ENEMY HEALTH AFTER DAMAGE: " + enemy.Health);
    }
}