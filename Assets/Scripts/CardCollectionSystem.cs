using UnityEngine;

public class CardCollectionSystem : MonoBehaviour
{
    public PlayerStats playerStats;
    public InventoryManger inventory;

    public int CardsRequired = 10;

    public int WolfXP = 100;
    public int GoblinXP = 100;
    public int SkeletonXP = 100;
    public int SlimeXP = 100;

    public void CheckCards()
    {
        if (inventory == null)
            inventory = InventoryManger.Instance;

        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();

        if (inventory == null || playerStats == null)
        {
            Debug.LogError("CARD COLLECTION → Missing Inventory or PlayerStats");
            return;
        }

        CheckEnemyCards("Wolf", WolfXP);
        CheckEnemyCards("Goblin", GoblinXP);
        CheckEnemyCards("Skeleton", SkeletonXP);
        CheckEnemyCards("Slime", SlimeXP);
    }

    private void CheckEnemyCards(string cardName, int xp)
    {
        int totalCards = 0;

        for (int i = 16; i < 25; i++)
        {
            InventorySlotData slot = inventory.Slots[i];

            if (slot == null || slot.Item == null)
                continue;

            if (slot.Item.ItemName != cardName)
                continue;

            totalCards += slot.Amount;
        }

        int sets = totalCards / CardsRequired;

        if (sets <= 0)
            return;

        int amountToRemove = sets * CardsRequired;

        RemoveCards(cardName, amountToRemove);

        int totalXP = sets * xp;

        playerStats.AddCardXP(totalXP);

        Debug.Log(
            "CARD SET → " +
            cardName +
            " | CARDS: " +
            amountToRemove +
            " | XP: " +
            totalXP
        );
    }

    private void RemoveCards(string cardName, int amount)
    {
        int remaining = amount;

        for (int i = 16; i < 25; i++)
        {
            if (remaining <= 0)
                break;

            InventorySlotData slot = inventory.Slots[i];

            if (slot == null || slot.Item == null)
                continue;

            if (slot.Item.ItemName != cardName)
                continue;

            int remove = Mathf.Min(
                slot.Amount,
                remaining
            );

            slot.Amount -= remove;
            remaining -= remove;

            if (slot.Amount <= 0)
            {
                slot.Amount = 0;
                slot.Item = null;
            }
        }

        if (InventoryUI.Instance != null)
            InventoryUI.Instance.Refresh();
    }
}