using UnityEngine;
using UnityEngine.AI;

public class SlowTime : MonoBehaviour
{
    public SkillData skillData;
    public float Duration = 5f;

    [Range(0.1f, 1f)]
    public float SlowMultiplier = 0.4f;

    private bool IsActive;
    private float cooldownTimer;

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

        IsActive = true;
        cooldownTimer = skillData.cooldown;

        EnemyAi[] enemies = FindObjectsByType<EnemyAi>(
            FindObjectsSortMode.None
        );

        foreach (EnemyAi enemy in enemies)
        {
            if (enemy == null)
                continue;

            if (enemy.agent == null)
                continue;

            enemy.agent.speed *= SlowMultiplier;
        }

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.Show(
                "SLOW TIME!",
                "All enemies have been slowed."
            );
        }

        CancelInvoke(nameof(EndSlowTime));
        Invoke(nameof(EndSlowTime), Duration);
    }

    private void EndSlowTime()
    {
        EnemyAi[] enemies = FindObjectsByType<EnemyAi>(
            FindObjectsSortMode.None
        );

        foreach (EnemyAi enemy in enemies)
        {
            if (enemy == null)
                continue;

            if (enemy.agent == null)
                continue;

            enemy.agent.speed /= SlowMultiplier;
        }

        IsActive = false;

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.Show(
                "SLOW TIME ENDED",
                "Enemies have returned to normal speed."
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