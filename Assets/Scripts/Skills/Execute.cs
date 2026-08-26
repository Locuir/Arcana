using UnityEngine;

public class Execute : MonoBehaviour
{
    public SkillData skillData;
    public float Duration = 10f;

    [Range(0f, 1f)]
    public float ExecuteThreshold = 0.2f;

    private bool IsActive;
    private float cooldownTimer;

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public void Activate()
    {
        Debug.Log("EXECUTE → Activate called");

        if (skillData == null)
        {
            Debug.LogError("EXECUTE → SkillData is NULL!");
            return;
        }

        if (!skillData.unlocked)
        {
            Debug.Log("EXECUTE → Skill is LOCKED!");
            return;
        }

        if (IsActive)
        {
            Debug.Log("EXECUTE → Already active!");
            return;
        }

        if (cooldownTimer > 0f)
        {
            Debug.Log("EXECUTE → On cooldown: " + cooldownTimer);
            return;
        }

        IsActive = true;
        cooldownTimer = skillData.cooldown;

        Debug.Log(
            "EXECUTE → ACTIVATED | Duration: " +
            Duration +
            " | Threshold: " +
            (ExecuteThreshold * 100f) +
            "%"
        );

        CancelInvoke(nameof(EndExecute));
        Invoke(nameof(EndExecute), Duration);
    }

    public bool TryExecute(EnemyStatus enemy)
    {
        if (!IsActive)
            return false;

        if (enemy == null)
            return false;

        float healthPercent =
            (float)enemy.Health / enemy.MaxHealth;

        Debug.Log(
            "EXECUTE → " +
            enemy.name +
            " | HP: " +
            enemy.Health +
            "/" +
            enemy.MaxHealth +
            " | HP%: " +
            (healthPercent * 100f)
        );

        if (healthPercent > ExecuteThreshold)
            return false;

        Debug.Log(
            "EXECUTE → KILLING " +
            enemy.name
        );

        enemy.TakeDamage(enemy.Health);

        return true;
    }

    private void EndExecute()
    {
        IsActive = false;

        Debug.Log("EXECUTE → ENDED");
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