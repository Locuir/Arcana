using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest Settings")]
    public int MaxSlots = 8;

    [Header("Chest Contents")]
    public InventorySlotData[] Slots;

    private void Awake()
    {
        MaxSlots = Mathf.Max(1, MaxSlots);

        if (Slots == null || Slots.Length != MaxSlots)
        {
            InventorySlotData[] oldSlots = Slots;

            Slots = new InventorySlotData[MaxSlots];

            for (int i = 0; i < MaxSlots; i++)
            {
                if (oldSlots != null &&
                    i < oldSlots.Length &&
                    oldSlots[i] != null)
                {
                    Slots[i] = oldSlots[i];
                }
                else
                {
                    Slots[i] = new InventorySlotData();
                }
            }
        }
        else
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i] == null)
                    Slots[i] = new InventorySlotData();
            }
        }
    }
    public bool Add(ItemData item)
    {
        if (item == null)
            return false;

        // Try stacking first
        if (item.Stackable)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].Item == null)
                    continue;

                if (Slots[i].Item.ID != item.ID)
                    continue;

                if (!Slots[i].Item.Stackable)
                    continue;

                if (Slots[i].Amount >= Slots[i].Item.MaxStack)
                    continue;

                Slots[i].Amount++;
                return true;
            }
        }

        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].Item == null)
            {
                Slots[i].Item = item;
                Slots[i].Amount = 1;
                return true;
            }
        }

        return false;
    }

    public void ClearSlot(int index)
    {
        if (index < 0 || index >= Slots.Length)
            return;

        Slots[index].Item = null;
        Slots[index].Amount = 0;
    }
}