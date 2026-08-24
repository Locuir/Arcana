using UnityEngine;

public class InventoryManger : MonoBehaviour
{
    public static InventoryManger Instance;

    [Header("Inventory")]
    public int MaxSlots = 25;

    public InventorySlotData[] Slots;

    private const int ItemStartIndex = 0;
    private const int ItemEndIndex = 16;

    private const int CardStartIndex = 16;
    private const int CardEndIndex = 25;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Slots = new InventorySlotData[MaxSlots];

        for (int i = 0; i < MaxSlots; i++)
        {
            Slots[i] = new InventorySlotData();
        }
    }

    public bool Add(ItemData item)
    {
        if (item == null)
            return false;

        int startIndex;
        int endIndex;

        if (item.Type == ItemType.Card)
        {
            startIndex = CardStartIndex;
            endIndex = CardEndIndex;
        }
        else
        {
            startIndex = ItemStartIndex;
            endIndex = ItemEndIndex;
        }

        if (item.Stackable)
        {
            if (TryStack(item, startIndex, endIndex))
                return true;
        }

        for (int i = startIndex; i < endIndex; i++)
        {
            if (Slots[i].Item == null)
            {
                Slots[i].Item = item;
                Slots[i].Amount = 1;

                RefreshInventory();
                return true;
            }
        }

        return false;
    }

    private bool TryStack(
        ItemData item,
        int startIndex,
        int endIndex)
    {
        for (int i = startIndex; i < endIndex; i++)
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

            RefreshInventory();
            return true;
        }

        return false;
    }

    public bool ContainsWeapon(WeaponData weapon)
    {
        if (weapon == null)
            return false;

        for (int i = ItemStartIndex; i < ItemEndIndex; i++)
        {
            if (Slots[i] == null ||
                Slots[i].Item == null)
                continue;

            if (Slots[i].Item == weapon)
                return true;
        }

        return false;
    }

    public bool RemoveWeapon(WeaponData weapon)
    {
        if (weapon == null)
            return false;

        for (int i = ItemStartIndex; i < ItemEndIndex; i++)
        {
            if (Slots[i] == null ||
                Slots[i].Item == null)
                continue;

            if (Slots[i].Item != weapon)
                continue;

            Slots[i].Item = null;
            Slots[i].Amount = 0;

            RefreshInventory();
            return true;
        }

        return false;
    }

    public bool AddWeapon(WeaponData weapon)
    {
        if (weapon == null)
            return false;

        return Add(weapon);
    }
    public void RefreshShopInventory()
    {
        RefreshInventory();
    }
    public void RefreshInventory()
    {
        if (InventoryUI.Instance != null)
            InventoryUI.Instance.Refresh();
    }
}