using UnityEngine;

public class BloodPrice : MonoBehaviour
{
    public SkillData skillData;
    public PlayerStats playerStats;

    [Header("Blood Price")]
    [Range(0f, 1f)]
    public float HealthCostPercent = 0.2f;

    public float DamageMultiplier = 2f;
    public float Duration = 8f;

    [Header("Effect")]
    public ParticleSystem BloodPriceParticle;

    private bool IsActive;
    private float cooldownTimer;

    private void Start()
    {
        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();

        if (BloodPriceParticle != null)
            BloodPriceParticle.Stop();
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer < 0f)
                cooldownTimer = 0f;
        }
    }

    public void Activate()
    {
        Debug.Log("BLOOD PRICE → Activate called");

        if (skillData == null)
        {
            Debug.LogError("BLOOD PRICE → SkillData is NULL!");
            return;
        }

        if (!skillData.unlocked)
        {
            Debug.Log("BLOOD PRICE → Skill is LOCKED!");
            return;
        }

        if (IsActive)
        {
            Debug.Log("BLOOD PRICE → Already active!");
            return;
        }

        if (cooldownTimer > 0f)
        {
            Debug.Log(
                "BLOOD PRICE → On cooldown: " +
                cooldownTimer
            );

            return;
        }

        if (playerStats == null)
        {
            Debug.LogError("BLOOD PRICE → PlayerStats is NULL!");
            return;
        }

        int healthCost = Mathf.RoundToInt(
            playerStats.MaxHP * HealthCostPercent
        );

        if (healthCost >= playerStats.CurrentHP)
        {
            Debug.Log(
                "BLOOD PRICE → Not enough HP!"
            );

            return;
        }

        playerStats.LoseHealth(healthCost);

        playerStats.SetDamageMultiplier(
            DamageMultiplier
        );

        IsActive = true;
        cooldownTimer = skillData.cooldown;

        if (BloodPriceParticle != null)
        {
            BloodPriceParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            BloodPriceParticle.Play();
        }

        Debug.Log(
            "BLOOD PRICE → ACTIVATED | " +
            "HP Cost: " +
            healthCost +
            " | Damage Multiplier: " +
            DamageMultiplier +
            " | Duration: " +
            Duration
        );

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.Show(
                "BLOOD PRICE!",
                "Sacrifice HP for overwhelming power."
            );
        }

        CancelInvoke(nameof(EndBloodPrice));
        Invoke(nameof(EndBloodPrice), Duration);
    }

    private void EndBloodPrice()
    {
        playerStats.ResetDamageMultiplier();

        IsActive = false;


        Debug.Log("BLOOD PRICE → ENDED");

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.Show(
                "BLOOD PRICE ENDED",
                "Your damage returned to normal."
            );
        }
    }

    public bool IsSkillReady()
    {
        return !IsActive && cooldownTimer <= 0f;
    }

    public bool IsSkillActive()
    {
        return IsActive;
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