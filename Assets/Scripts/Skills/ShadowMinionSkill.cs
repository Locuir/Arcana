using UnityEngine;

public class ShadowMinionSkill : MonoBehaviour
{
    [Header("Skill")]
    public SkillData skillData;

    [Header("Minion")]
    public GameObject minionPrefab;
    public float Duration = 10f;

    [Header("References")]
    public PlayerStats playerStats;

    private float cooldownTimer;
    private bool isActive;

    private void Start()
    {
        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();
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

        if (cooldownTimer > 0f)
            return;

        if (minionPrefab == null)
        {
            Debug.LogError(
                "SHADOW MINION → Minion Prefab is NULL!"
            );
            return;
        }

        if (playerStats == null)
        {
            Debug.LogError(
                "SHADOW MINION → PlayerStats not found!"
            );
            return;
        }

        Vector3 spawnPosition =
            playerStats.transform.position +
            playerStats.transform.forward * 2f;

        GameObject minion =
            Instantiate(
                minionPrefab,
                spawnPosition,
                playerStats.transform.rotation
            );

        ShadowMinion shadowMinion =
            minion.GetComponent<ShadowMinion>();

        if (shadowMinion != null)
        {
            shadowMinion.Duration = Duration;
            shadowMinion.playerStats = playerStats;
        }

        cooldownTimer = skillData.cooldown;

        Debug.Log(
            "SHADOW MINION → SUMMONED"
        );
    }

    public bool IsSkillReady()
    {
        return cooldownTimer <= 0f;
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