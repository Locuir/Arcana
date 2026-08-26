using UnityEngine;

public class Vampirism : MonoBehaviour
{
    public SkillData skillData;
    public PlayerStatus playerStatus;

    [Header("Vampirism")]
    public float Duration = 10f;
    [Range(0f, 1f)]
    public float LifeStealPercent = 0.2f;

    [Header("Effect")]
    public ParticleSystem VampirismParticle;

    private bool IsActive;
    private float cooldownTimer;

    private void Start()
    {
        if (playerStatus == null)
            playerStatus = GetComponent<PlayerStatus>();

        if (VampirismParticle != null)
            VampirismParticle.Stop();
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

        if (VampirismParticle != null)
            VampirismParticle.Play();

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.Show(
                "VAMPIRISM!",
                "Your attacks restore health."
            );
        }

        CancelInvoke(nameof(EndVampirism));
        Invoke(nameof(EndVampirism), Duration);
    }

    public void OnDamageDealt(int damage)
    {
        if (!IsActive)
            return;

        if (damage <= 0)
            return;

        int healAmount = Mathf.RoundToInt(
            damage * LifeStealPercent
        );

        if (healAmount <= 0)
            return;

        playerStatus.Heal(healAmount);

        Debug.Log(
            "VAMPIRISM → Damage: " +
            damage +
            " | Healed: " +
            healAmount
        );
    }

    private void EndVampirism()
    {
        IsActive = false;

        if (VampirismParticle != null)
            VampirismParticle.Stop();

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.Show(
                "VAMPIRISM ENDED",
                "Your attacks no longer restore health."
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