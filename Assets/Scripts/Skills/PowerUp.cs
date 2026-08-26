using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public SkillData skillData;
    public PlayerStats playerStats;

    [Header("Power Up")]
    public int StrengthBonus = 3;
    public int VigorBonus = 3;
    public float Duration = 60f;
    public int animationSkillID = 1;

    [Header("Effect")]
    public ParticleSystem PowerUpParticle;

    [Header("References")]
    public Animator animator;

    private bool IsActive;
    private float cooldownTimer;

    private void Start()
    {
        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();

        if (PowerUpParticle != null)
            PowerUpParticle.Stop();
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public void Activate()
    {
        if (skillData == null)
            return;

        if (!skillData.unlocked)
            return;

        if (IsActive)
            return;

        if (cooldownTimer > 0f)
            return;

        if (playerStats == null)
            return;

        IsActive = true;
        cooldownTimer = skillData.cooldown;

        if (PowerUpParticle != null)
            PowerUpParticle.Play();

        playerStats.ActivateTemporaryStats(
            StrengthBonus,
            VigorBonus
        );

        if (animator != null)
        {
            animator.SetInteger("SkillID", animationSkillID);
            animator.SetTrigger("SkillTrigger");
        }

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.Show(
                "POWER UP!",
                "Strength +" + StrengthBonus + " | Vigor +" + VigorBonus
            );
        }

        CancelInvoke(nameof(EndPowerUp));
        Invoke(nameof(EndPowerUp), Duration);
    }

    private void EndPowerUp()
    {
        if (playerStats != null)
            playerStats.RemoveTemporaryStats();

        IsActive = false;

        if (PowerUpParticle != null)
            PowerUpParticle.Stop();

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.Show(
                "POWER UP ENDED",
                "Your stats returned to normal."
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