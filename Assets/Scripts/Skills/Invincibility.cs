using UnityEngine;

public class Invincibility : MonoBehaviour
{
    public SkillData skillData;
    public PlayerStatus playerStatus;

    [Header("Skill")]
    public float Duration = 5f;

    [Header("Effect")]
    public TrailRenderer[] invincibilityTrails;

    private bool IsActive;
    private float cooldownTimer;

    private void Start()
    {
        Debug.Log("INVINCIBILITY → Start");

        if (playerStatus == null)
        {
            playerStatus = GetComponent<PlayerStatus>();

            if (playerStatus != null)
                Debug.Log("INVINCIBILITY → PlayerStatus found");
            else
                Debug.LogError("INVINCIBILITY → PlayerStatus NOT FOUND!");
        }

        if (skillData == null)
            Debug.LogError("INVINCIBILITY → SkillData NOT ASSIGNED!");

        if (invincibilityTrails == null || invincibilityTrails.Length == 0)
        {
            Debug.LogError("INVINCIBILITY → No Trails assigned!");
        }
        else
        {
            Debug.Log("INVINCIBILITY → Trails found: " + invincibilityTrails.Length);
        }

        SetTrails(false);
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

        if (skillData == null)
        {
            return;
        }



        if (!skillData.unlocked)
        {
            return;
        }

        if (IsActive)
        {
            return;
        }

        if (cooldownTimer > 0f)
        {

            return;
        }

        if (playerStatus == null)
        {
            return;
        }

        IsActive = true;
        cooldownTimer = skillData.cooldown;

        playerStatus.SetInvulnerable(true);


        SetTrails(true);

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.Show(
                "INVINCIBLE!",
                "You cannot take damage."
            );
        }

        CancelInvoke(nameof(EndInvincibility));
        Invoke(nameof(EndInvincibility), Duration);
    }

    private void EndInvincibility()
    {

        if (playerStatus != null)
            playerStatus.SetInvulnerable(false);

        SetTrails(false);

        IsActive = false;


        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.Show(
                "INVINCIBILITY ENDED",
                "You can take damage again."
            );
        }
    }

    private void SetTrails(bool state)
    {
        if (invincibilityTrails == null)
            return;

        foreach (TrailRenderer trail in invincibilityTrails)
        {
            if (trail != null)
            {
                trail.emitting = state;

            }
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