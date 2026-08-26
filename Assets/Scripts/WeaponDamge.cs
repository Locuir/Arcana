using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public Collider WeaponHitBox;
    public WeaponData WeaponData;
    private Vampirism vampirism;
    private MadnessOfCrit madnessOfCrit;
    private Execute execute;
    private HashSet<EnemyStatus> EnemiesHit =
        new HashSet<EnemyStatus>();

    private PlayerStats playerStats;

    private void Awake()
    {
        if (WeaponHitBox != null)
            WeaponHitBox.enabled = false;

        playerStats = FindFirstObjectByType<PlayerStats>();
        vampirism = FindFirstObjectByType<Vampirism>();
        execute = FindFirstObjectByType<Execute>();
        madnessOfCrit = FindFirstObjectByType<MadnessOfCrit>();
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

        if (execute != null)
        {
            if (execute.TryExecute(enemy))
                return;
        }


        enemy.TakeDamage(finalDamage);
        if (vampirism != null)
            vampirism.OnDamageDealt(finalDamage);



    }

    private int CalculateDamage()
    {
        if (WeaponData == null)
        {
            return 0;
        }

        if (playerStats == null)
        {
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

        damage = Mathf.RoundToInt(
            damage * playerStats.DamageMultiplier
        );

        bool guaranteedCrit =
            madnessOfCrit != null &&
            madnessOfCrit.IsCritGuaranteed();

        bool normalCrit =
            Random.Range(0f, 100f) < playerStats.CritChacne;

        if (guaranteedCrit || normalCrit)
        {
            damage *= 2;

            Debug.Log(
                guaranteedCrit
                    ? "MADNESS OF CRIT → GUARANTEED CRITICAL! Damage = " + damage
                    : "CRITICAL HIT! Damage = " + damage
            );
        }
        return damage;
    }
}