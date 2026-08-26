using UnityEngine;

public class Heal : MonoBehaviour
{
    public PlayerStatus playerStatus;

    [Header("Heal")]
    public SkillData skillData;
    public float HealthRegenBonus = 5f;
    public float Duration = 20f;
    public int animationSkillID = 2;
    public ParticleSystem HealParticles;

    private bool IsActive;
    private float cooldownTimer;

    private float OriginalHealthRegen;

    private void Start()
    {
        if (playerStatus == null)
            playerStatus = GetComponent<PlayerStatus>();

        if (playerStatus != null)
            OriginalHealthRegen = playerStatus.HealthRegen;
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

        if (playerStatus == null)
            return;

        IsActive = true;

        cooldownTimer = skillData.cooldown;

        OriginalHealthRegen =
            playerStatus.HealthRegen;

        playerStatus.HealthRegen +=
            HealthRegenBonus;

        if (HealParticles != null)
            HealParticles.Play();

        NotificationManager.Instance.Show(
            "HEAL!",
            "Health regeneration increased!"
        );

        CancelInvoke(nameof(EndHeal));
        Invoke(nameof(EndHeal), Duration);
    }
    private void EndHeal()
    {
        if (playerStatus != null)
            playerStatus.HealthRegen =
                OriginalHealthRegen;

        if (HealParticles != null)
            HealParticles.Stop();

        IsActive = false;


        NotificationManager.Instance.Show(
            "HEAL ENDED",
            "Health regeneration returned to normal."
        );
    }

    public bool IsSkillReady()
    {
        return !IsActive &&
               cooldownTimer <= 0f;
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
            cooldownTimer /
            skillData.cooldown
        );
    }
}