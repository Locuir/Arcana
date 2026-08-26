using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    public PlayerStats Stats;

    [Header("Health")]
    public float Health;
    public int MaxHealth;

    [Header("Health Regeneration")]
    public float HealthRegen = .5f;
    [Header("Starting Weapon")]
    public WeaponData StartingWeapon;
    private bool IsDead;

    public bool IsInvulnerable { get; private set; }



    private void Start()
    {
        if (Stats == null)
            Stats = GetComponent<PlayerStats>();

        if (Stats == null)
        {
            Debug.LogError("PLAYER STATUS → PlayerStats NOT FOUND!");
            return;
        }

        MaxHealth = Stats.MaxHP;
        Health = MaxHealth;

        Stats.CurrentHP = MaxHealth;
    }

    private void Update()
    {
        RegenerateHealth();
    }


    public void SetInvulnerable(bool value)
    {
        IsInvulnerable = value;

        Debug.Log(
            "PLAYER INVULNERABILITY → " +
            (value ? "ON" : "OFF")
        );
    }
    private void RegenerateHealth()
    {
        if (IsDead)
            return;

        if (Health <= 0)
            return;

        if (Health >= MaxHealth)
            return;

        Health += HealthRegen * Time.deltaTime;

        if (Health > MaxHealth)
            Health = MaxHealth;

        Stats.CurrentHP = Mathf.RoundToInt(Health);
    }

    public void TakeDamage(int DamageTaken)
    {
        if (IsDead)
            return;

        if (IsInvulnerable)
            return;

        if (DamageTaken <= 0)
            return;

        Health -= DamageTaken;

        if (Health < 0)
            Health = 0;

        Stats.CurrentHP = Mathf.RoundToInt(Health);

        Debug.Log(
            "PLAYER DAMAGE → " +
            DamageTaken +
            " | HP: " +
            Health +
            "/" +
            MaxHealth
        );

        CheckDeath();
    }

    public void Heal(int Amount)
    {
        if (IsDead)
            return;

        if (Amount <= 0)
            return;

        Health += Amount;

        if (Health > MaxHealth)
            Health = MaxHealth;

        Stats.CurrentHP = Mathf.RoundToInt(Health);
    }

    private void CheckDeath()
    {
        if (IsDead)
            return;

        if (Health <= 0)
        {
            RunManager.Instance.EndRun();
            IsDead = true;

            Debug.Log("PLAYER DEAD → RESTARTING");


        }
    }
}