using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public PlayerStats playerStats;
    public PlayerStatsUI playerStatsUI;

    public void UpgradeVigor()
    {
        if (!CanUpgrade())
            return;

        playerStats.IncreaseVigor();
        SpendPoint();
    }

    public void UpgradeEndurance()
    {
        if (!CanUpgrade())
            return;

        playerStats.IncreaseEndurance();
        SpendPoint();
    }

    public void UpgradeStrength()
    {
        if (!CanUpgrade())
            return;

        playerStats.IncreaseStrength();
        SpendPoint();
    }

    public void UpgradeDexterity()
    {
        if (!CanUpgrade())
            return;

        playerStats.IncreaseDexterity();
        SpendPoint();
    }

    public void UpgradeLuck()
    {
        if (!CanUpgrade())
            return;

        playerStats.IncreaseLuck();
        SpendPoint();
    }

    public void UpgradeFaith()
    {
        if (!CanUpgrade())
            return;

        playerStats.IncreaseFaith();
        SpendPoint();
    }

    public void UpgradeVitality()
    {
        if (!CanUpgrade())
            return;

        playerStats.IncreaseVitality();
        SpendPoint();
    }

    private bool CanUpgrade()
    {
        return playerStats != null &&
               playerStats.AvailableStatPoints > 0;
    }

    private void SpendPoint()
    {
        playerStats.AvailableStatPoints--;

        playerStats.CalculateDrivedStats();

        if (playerStatsUI != null)
            playerStatsUI.Refresh();
    }
}