using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public Collider WeaponHitBox;
    public WeaponData WeaponData;

    private HashSet<EnemyStatus> EnemiesHit =
        new HashSet<EnemyStatus>();

    private PlayerStats playerStats;

    private void Awake()
    {
        if (WeaponHitBox != null)
            WeaponHitBox.enabled = false;

        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    public void EnableHitBox()
    {
        EnemiesHit.Clear();

        if (WeaponHitBox != null)
            WeaponHitBox.enabled = true;
    }

    public void DisableHitBox()
    {
        if (WeaponHitBox != null)
            WeaponHitBox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDealDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (WeaponHitBox != null && WeaponHitBox.enabled)
            TryDealDamage(other);
    }

    private void TryDealDamage(Collider other)
    {
        EnemyStatus enemy =
            other.GetComponentInParent<EnemyStatus>();

        if (enemy == null)
            return;

        if (EnemiesHit.Contains(enemy))
            return;

        EnemiesHit.Add(enemy);

        int finalDamage = CalculateDamage();

        enemy.TakeDamage(finalDamage);
    }

    private int CalculateDamage()
    {
        if (WeaponData == null)
        {
            Debug.LogError("WeaponData is NULL!");
            return 0;
        }

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found!");
            return WeaponData.damage;
        }

        float strengthDamage =
            playerStats.Strength *
            WeaponData.strengthScaling;

        float dexterityDamage =
            playerStats.Dexterity *
            WeaponData.dexterityScaling;

        int damage = Mathf.RoundToInt(
            WeaponData.damage +
            strengthDamage +
            dexterityDamage
        );

        // Critical Strike
        if (Random.Range(0f, 100f) < playerStats.CritChacne)
        {
            damage *= 2;

            Debug.Log(
                "CRITICAL HIT! Damage = " + damage
            );
        }

        return damage;
    }
}