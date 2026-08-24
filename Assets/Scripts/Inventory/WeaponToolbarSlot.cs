using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponToolbarSlot : MonoBehaviour,
    IDropHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public Image WeaponIcon;
    public Image SelectedImage;

    public int SlotIndex;

    private WeaponData weapon;

    private Canvas canvas;
    private GameObject dragIcon;
    private bool droppedSuccessfully;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        if (WeaponIcon != null)
        {
            WeaponIcon.sprite = null;
            WeaponIcon.enabled = false;
        }

        if (SelectedImage != null)
            SelectedImage.enabled = false;
    }

    public WeaponData GetWeapon()
    {
        return weapon;
    }

    public void SetWeapon(WeaponData newWeapon)
    {
        weapon = newWeapon;

        if (weapon == null)
        {
            ClearSlot();
            return;
        }

        if (WeaponIcon != null)
        {
            WeaponIcon.sprite = weapon.Icon;
            WeaponIcon.enabled = true;
        }
    }

    public void ClearSlot()
    {
        weapon = null;

        if (WeaponIcon != null)
        {
            WeaponIcon.sprite = null;
            WeaponIcon.enabled = false;
        }

        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        if (SelectedImage != null)
            SelectedImage.enabled = selected;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        InventoryDrag inventoryDrag =
            eventData.pointerDrag.GetComponent<InventoryDrag>();

        if (inventoryDrag != null)
        {
            InventorySlot sourceSlot = inventoryDrag.Slot;

            if (sourceSlot == null)
                return;

            if (sourceSlot.Owner != InventorySlot.SlotOwner.Inventory)
                return;

            int index = sourceSlot.SlotIndex;

            if (InventoryManger.Instance == null)
                return;

            if (index < 0 ||
                index >= InventoryManger.Instance.Slots.Length)
                return;

            InventorySlotData data =
                InventoryManger.Instance.Slots[index];

            if (data == null ||
                data.Item == null)
                return;

            WeaponData droppedWeapon =
                data.Item as WeaponData;

            if (droppedWeapon == null)
                return;

            SetWeapon(droppedWeapon);

            data.Item = null;
            data.Amount = 0;

            inventoryDrag.SetDropSuccessful();

            if (InventoryUI.Instance != null)
                InventoryUI.Instance.RefreshAll();

            return;
        }

        WeaponToolbarSlot toolbarDrag =
            eventData.pointerDrag.GetComponent<WeaponToolbarSlot>();

        if (toolbarDrag != null)
        {
            if (toolbarDrag == this)
                return;

            WeaponData droppedWeapon =
                toolbarDrag.GetWeapon();

            if (droppedWeapon == null)
                return;

            SetWeapon(droppedWeapon);
            toolbarDrag.ClearSlot();
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (weapon == null)
            return;

        if (canvas == null)
            return;

        droppedSuccessfully = false;

        dragIcon = new GameObject("ToolbarDragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling();

        Image image =
            dragIcon.AddComponent<Image>();

        image.sprite = weapon.Icon;
        image.raycastTarget = false;
        image.preserveAspect = true;

        RectTransform rect =
            dragIcon.GetComponent<RectTransform>();

        rect.sizeDelta =
            WeaponIcon.rectTransform.rect.size;

        rect.position =
            eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon == null)
            return;

        dragIcon.GetComponent<RectTransform>().position =
            eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
            Destroy(dragIcon);

        if (!droppedSuccessfully)
            return;
    }
}