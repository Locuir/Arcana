using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image Icon;
    public TMP_Text AmountText;
    public int SlotIndex = -1;
    public InventorySlotData CurrentSlot;
    public SlotOwner Owner;

    public enum SlotOwner
    {
        Inventory,
        Chest,
    }

    private void Start()
    {
        Debug.Log($"[InventorySlot] START | {gameObject.name} | Index={SlotIndex} | Owner={Owner}");

        Icon.enabled = false;

        if (AmountText != null)
            AmountText.gameObject.SetActive(false);
    }

    public void SetItem(InventorySlotData slot)
    {
        Debug.Log(
            $"[SLOT SETITEM] {gameObject.name} | " +
            $"Index={SlotIndex} | " +
            $"Owner={Owner} | " +
            $"Item={(slot != null && slot.Item != null ? slot.Item.ItemName : "EMPTY")}"
        );

        CurrentSlot = slot;

        if (slot == null || slot.Item == null)
        {
            Icon.enabled = false;

            if (AmountText != null)
                AmountText.gameObject.SetActive(false);

            return;
        }

        Icon.enabled = true;
        Icon.sprite = slot.Item.Icon;

        if (AmountText != null)
        {
            if (slot.Amount > 1)
            {
                AmountText.gameObject.SetActive(true);
                AmountText.text = slot.Amount.ToString();
            }
            else
            {
                AmountText.gameObject.SetActive(false);
            }
        }
    }
}