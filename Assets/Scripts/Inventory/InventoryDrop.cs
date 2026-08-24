using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : MonoBehaviour, IDropHandler
{
    public InventorySlot Slot;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || Slot == null)
            return;

        InventoryDrag inventoryDrag =
            eventData.pointerDrag.GetComponent<InventoryDrag>();

        if (inventoryDrag != null)
        {
            HandleInventoryDrag(inventoryDrag);
            return;
        }

        WeaponToolbarSlot toolbarSlot =
            eventData.pointerDrag.GetComponent<WeaponToolbarSlot>();

        if (toolbarSlot != null)
        {
            HandleToolbarDrag(toolbarSlot);
        }
    }

    private void HandleInventoryDrag(InventoryDrag drag)
    {
        InventorySlot from = drag.Slot;

        if (from == null)
            return;

        if (from.Owner == InventorySlot.SlotOwner.Inventory &&
            Slot.Owner == InventorySlot.SlotOwner.Inventory)
        {
            SwapInventory(from, Slot);
            drag.SetDropSuccessful();
            return;
        }

        if (from.Owner == InventorySlot.SlotOwner.Inventory &&
            Slot.Owner == InventorySlot.SlotOwner.Chest)
        {
            if (ChestUI.Instance != null &&
                ChestUI.Instance.MoveInventoryToChest(from, Slot))
            {
                drag.SetDropSuccessful();
            }

            return;
        }

        if (from.Owner == InventorySlot.SlotOwner.Chest &&
            Slot.Owner == InventorySlot.SlotOwner.Inventory)
        {
            if (ChestUI.Instance != null &&
                ChestUI.Instance.MoveChestToInventory(from, Slot))
            {
                drag.SetDropSuccessful();
            }

            return;
        }

        if (from.Owner == InventorySlot.SlotOwner.Chest &&
            Slot.Owner == InventorySlot.SlotOwner.Chest)
        {
            if (ChestUI.Instance != null &&
                ChestUI.Instance.HandleChestSlotDrop(from, Slot))
            {
                drag.SetDropSuccessful();
            }
        }
    }

    private void HandleToolbarDrag(
        WeaponToolbarSlot toolbarSlot)
    {
        WeaponData weapon = toolbarSlot.GetWeapon();

        if (weapon == null)
            return;

        if (InventoryManger.Instance == null)
            return;

        if (Slot.Owner == InventorySlot.SlotOwner.Inventory)
        {
            if (!InventoryManger.Instance.Add(weapon))
                return;

            toolbarSlot.ClearSlot();

            if (InventoryUI.Instance != null)
                InventoryUI.Instance.RefreshAll();

            return;
        }

        if (Slot.Owner == InventorySlot.SlotOwner.Chest)
        {
            if (ChestUI.Instance == null)
                return;

            if (!ChestUI.Instance.AddWeapon(weapon))
                return;

            toolbarSlot.ClearSlot();

            if (InventoryUI.Instance != null)
                InventoryUI.Instance.RefreshAll();

            return;
        }
    }

    private void SwapInventory(
        InventorySlot fromSlot,
        InventorySlot toSlot)
    {
        if (InventoryManger.Instance == null)
            return;

        int from = fromSlot.SlotIndex;
        int to = toSlot.SlotIndex;

        if (from == to)
            return;

        if (from < 0 ||
            from >= InventoryManger.Instance.Slots.Length)
            return;

        if (to < 0 ||
            to >= InventoryManger.Instance.Slots.Length)
            return;

        bool fromIsCard = from >= 16;
        bool toIsCard = to >= 16;

        if (fromIsCard != toIsCard)
            return;

        InventorySlotData temp =
            InventoryManger.Instance.Slots[from];

        InventoryManger.Instance.Slots[from] =
            InventoryManger.Instance.Slots[to];

        InventoryManger.Instance.Slots[to] =
            temp;

        if (InventoryUI.Instance != null)
            InventoryUI.Instance.RefreshAll();
    }
}