using UnityEngine;

public class MadnessOfCrit : MonoBehaviour
{
    public SkillData skillData;

    [Header("Skill")]
    public float Duration = 5f;

    [Header("Effect")]
    public ParticleSystem LeftCritParticle;
    public ParticleSystem RightCritParticle;

    private bool IsActive;
    private float cooldownTimer;

    private void Start()
    {
        if (LeftCritParticle != null)
            LeftCritParticle.Stop();
        if (RightCritParticle != null)
            RightCritParticle.Stop();
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public void Activate()
    {
        Debug.Log("MADNESS OF CRIT → Activate called");

        if (skillData == null)
        {
            Debug.LogError("MADNESS OF CRIT → SkillData is NULL!");
            return;
        }

        if (!skillData.unlocked)
        {
            Debug.Log("MADNESS OF CRIT → Skill is LOCKED!");
            return;
        }

        if (IsActive)
        {
            Debug.Log("MADNESS OF CRIT → Already active!");
            return;
        }

        if (cooldownTimer > 0f)
        {
            Debug.Log(
                "MADNESS OF CRIT → On cooldown: " +
                cooldownTimer
            );
            return;
        }

        IsActive = true;
        cooldownTimer = skillData.cooldown;

        if (LeftCritParticle != null)
            LeftCritParticle.Play();
        if (RightCritParticle != null)
            RightCritParticle.Play();

        Debug.Log(
            "MADNESS OF CRIT → ACTIVATED | Duration: " +
            Duration
        );

        CancelInvoke(nameof(EndMadness));
        Invoke(nameof(EndMadness), Duration);
    }

    private void EndMadness()
    {
        IsActive = false;

        if (LeftCritParticle != null)
            LeftCritParticle.Stop();
        if (RightCritParticle != null)
            RightCritParticle.Stop();

        Debug.Log("MADNESS OF CRIT → ENDED");
    }

    public bool IsCritGuaranteed()
    {
        return IsActive;
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