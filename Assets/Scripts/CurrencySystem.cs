using UnityEngine;

public class CurrencySystem : MonoBehaviour
{

    public PlayerStatsUI playerStatsUI;
    public int Essence { get; private set; }

    public void AddEssence(int amount)
    {
        if (amount <= 0)
            return;

        Essence += amount;
        playerStatsUI.Refresh();

    }

    public bool SpendEssence(int amount)
    {
        if (amount <= 0)
            return false;

        if (Essence < amount)
            return false;

        Essence -= amount;
        playerStatsUI.Refresh();

        return true;
    }

    public bool HasEnoughEssence(int amount)
    {
        return Essence >= amount;

    }

    public int GetEssence()
    {
        return Essence;
    }
}