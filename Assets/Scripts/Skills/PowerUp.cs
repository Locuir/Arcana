using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PlayerStats playerStats;

    [Header("Power Up")]
    public int StrengthBonus = 3;
    public int VigorBonus = 3;
    public float Duration = 60f;
    public int WaveCooldown = 2;
    public int animationSkillID = 1;

    public Animator animator;
    private bool IsActive;
    private bool IsReady = true;
    private int WavesRemaining;

    private void Start()
    {
        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();

        if (WaveManager.Instance != null)
            WaveManager.Instance.WaveCompletedEvent += OnWaveCompleted;
    }

    private void OnDestroy()
    {
        if (WaveManager.Instance != null)
            WaveManager.Instance.WaveCompletedEvent -= OnWaveCompleted;
    }

    public void Activate()
    {
        if (!IsReady)
            return;

        if (IsActive)
            return;

        IsActive = true;
        IsReady = false;

        playerStats.ActivateTemporaryStats(
            StrengthBonus,
            VigorBonus
        );
        if (animator != null)
        {
            animator.SetInteger("SkillID", animationSkillID);
            animator.SetTrigger("SkillTrigger");
        }

        NotificationManager.Instance.Show(
            "POWER UP!",
            "Strength +3 | Vigor +3"
        );

        CancelInvoke(nameof(EndPowerUp));
        Invoke(nameof(EndPowerUp), Duration);
    }

    private void EndPowerUp()
    {
        playerStats.RemoveTemporaryStats();

        IsActive = false;
        WavesRemaining = WaveCooldown;

        NotificationManager.Instance.Show(
            "POWER UP ENDED",
            "Your stats returned to normal."
        );
    }

    private void OnWaveCompleted()
    {
        if (IsActive)
            return;

        if (IsReady)
            return;

        if (WavesRemaining <= 0)
            return;

        WavesRemaining--;

        if (WavesRemaining <= 0)
        {
            IsReady = true;

            NotificationManager.Instance.Show(
                "POWER UP READY!",
                "Your skill is ready to use."
            );
        }
    }

    public bool IsSkillReady()
    {
        return IsReady && !IsActive;
    }

    public bool IsSkillActive()
    {
        return IsActive;
    }

    public int GetWavesRemaining()
    {
        return WavesRemaining;
    }
}