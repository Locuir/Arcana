using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public static ChestUI Instance;

    public GameObject ChestPanel;
    public InventorySlot[] ChestSlots;

    private Chest currentChest;

    private void Awake()
    {
        Instance = this;

        if (ChestPanel != null)
            ChestPanel.SetActive(false);
    }

    public void OpenChest(Chest chest)
    {
        if (chest == null)
            return;

        currentChest = chest;

        if (ChestPanel != null)
            ChestPanel.SetActive(true);

        Refresh();
    }

    public void CloseChest()
    {
        currentChest = null;

        if (ChestPanel != null)
            ChestPanel.SetActive(false);
    }

    public void Refresh()
    {
        if (currentChest == null)
            return;

        for (int i = 0; i < ChestSlots.Length; i++)
        {
            if (ChestSlots[i] == null)
                continue;

            ChestSlots[i].Owner =
                InventorySlot.SlotOwner.Chest;

            ChestSlots[i].SlotIndex = i;

            if (i >= currentChest.Slots.Length)
            {
                ChestSlots[i].SetItem(null);
                continue;
            }

            ChestSlots[i].SetItem(
                currentChest.Slots[i]
            );
        }
    }

    public Chest GetCurrentChest()
    {
        return currentChest;
    }

    public bool MoveInventoryToChest(
        InventorySlot draggedSlot,
        InventorySlot targetSlot)
    {
        if (currentChest == null ||
            draggedSlot == null ||
            targetSlot == null ||
            draggedSlot.CurrentSlot == null ||
            draggedSlot.CurrentSlot.Item == null)
            return false;

        int chestIndex = targetSlot.SlotIndex;

        if (chestIndex < 0 ||
            chestIndex >= currentChest.Slots.Length)
            return false;

        InventorySlotData from =
            draggedSlot.CurrentSlot;

        InventorySlotData to =
            currentChest.Slots[chestIndex];

        if (to.Item != null &&
            to.Item.ID == from.Item.ID &&
            to.Item.Stackable)
        {
            int space =
                to.Item.MaxStack - to.Amount;

            if (space <= 0)
                return false;

            int amount =
                Mathf.Min(from.Amount, space);

            to.Amount += amount;
            from.Amount -= amount;

            if (from.Amount <= 0)
            {
                from.Item = null;
                from.Amount = 0;
            }

            RefreshAll();
            return true;
        }

        if (to.Item != null)
            return false;

        to.Item = from.Item;
        to.Amount = from.Amount;

        from.Item = null;
        from.Amount = 0;

        RefreshAll();
        return true;
    }

    public bool MoveChestToInventory(
        InventorySlot draggedSlot,
        InventorySlot targetSlot)
    {
        if (currentChest == null ||
            draggedSlot == null ||
            targetSlot == null ||
            draggedSlot.CurrentSlot == null ||
            draggedSlot.CurrentSlot.Item == null)
            return false;

        int chestIndex = draggedSlot.SlotIndex;

        if (chestIndex < 0 ||
            chestIndex >= currentChest.Slots.Length)
            return false;

        InventorySlotData from =
            currentChest.Slots[chestIndex];

        InventorySlotData to =
            targetSlot.CurrentSlot;

        if (to == null)
            return false;

        if (to.Item != null &&
            to.Item.ID == from.Item.ID &&
            to.Item.Stackable)
        {
            int space =
                to.Item.MaxStack - to.Amount;

            if (space <= 0)
                return false;

            int amount =
                Mathf.Min(from.Amount, space);

            to.Amount += amount;
            from.Amount -= amount;

            if (from.Amount <= 0)
            {
                from.Item = null;
                from.Amount = 0;
            }

            RefreshAll();
            return true;
        }

        if (to.Item != null)
            return false;

        to.Item = from.Item;
        to.Amount = from.Amount;

        from.Item = null;
        from.Amount = 0;

        RefreshAll();
        return true;
    }

    public bool HandleChestSlotDrop(
        InventorySlot draggedSlot,
        InventorySlot targetSlot)
    {
        if (currentChest == null ||
            draggedSlot == null ||
            targetSlot == null ||
            draggedSlot.CurrentSlot == null ||
            draggedSlot.CurrentSlot.Item == null)
            return false;

        int fromIndex = draggedSlot.SlotIndex;
        int toIndex = targetSlot.SlotIndex;

        if (fromIndex < 0 ||
            fromIndex >= currentChest.Slots.Length)
            return false;

        if (toIndex < 0 ||
            toIndex >= currentChest.Slots.Length)
            return false;

        if (fromIndex == toIndex)
            return false;

        InventorySlotData from =
            currentChest.Slots[fromIndex];

        InventorySlotData to =
            currentChest.Slots[toIndex];

        if (to.Item != null &&
            to.Item.ID == from.Item.ID &&
            to.Item.Stackable)
        {
            int space =
                to.Item.MaxStack - to.Amount;

            if (space <= 0)
                return false;

            int amount =
                Mathf.Min(from.Amount, space);

            to.Amount += amount;
            from.Amount -= amount;

            if (from.Amount <= 0)
            {
                from.Item = null;
                from.Amount = 0;
            }

            Refresh();
            return true;
        }

        ItemData tempItem = to.Item;
        int tempAmount = to.Amount;

        to.Item = from.Item;
        to.Amount = from.Amount;

        from.Item = tempItem;
        from.Amount = tempAmount;

        Refresh();
        return true;
    }

    public bool AddWeapon(WeaponData weapon)
    {
        if (currentChest == null ||
            weapon == null)
            return false;

        if (!weapon.Stackable)
        {
            for (int i = 0; i < currentChest.Slots.Length; i++)
            {
                if (currentChest.Slots[i].Item == null)
                {
                    currentChest.Slots[i].Item = weapon;
                    currentChest.Slots[i].Amount = 1;

                    Refresh();
                    return true;
                }
            }

            return false;
        }

        return currentChest.Add(weapon);
    }

    public bool RemoveWeapon(WeaponData weapon)
    {
        if (currentChest == null ||
            weapon == null)
            return false;

        for (int i = 0; i < currentChest.Slots.Length; i++)
        {
            if (currentChest.Slots[i] == null ||
                currentChest.Slots[i].Item == null)
                continue;

            if (currentChest.Slots[i].Item != weapon)
                continue;

            currentChest.Slots[i].Item = null;
            currentChest.Slots[i].Amount = 0;

            Refresh();
            return true;
        }

        return false;
    }

    private void RefreshAll()
    {
        if (InventoryUI.Instance != null)
            InventoryUI.Instance.RefreshAll();
        else
            Refresh();
    }
}