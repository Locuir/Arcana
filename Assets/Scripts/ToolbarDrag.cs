using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolbarDrag : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public WeaponToolbarSlot Slot;

    private Canvas canvas;
    private GameObject dragIcon;
    private bool droppedSuccessfully;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        if (Slot == null)
            Slot = GetComponent<WeaponToolbarSlot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Slot == null)
            return;

        WeaponData weapon = Slot.GetWeapon();

        if (weapon == null)
            return;

        droppedSuccessfully = false;

        dragIcon = new GameObject("ToolbarDragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling();

        Image image = dragIcon.AddComponent<Image>();
        image.sprite = weapon.Icon;
        image.raycastTarget = false;

        RectTransform rect =
            dragIcon.GetComponent<RectTransform>();

        RectTransform slotRect =
            Slot.GetComponent<RectTransform>();

        rect.sizeDelta = slotRect.rect.size;
        rect.position = eventData.position;

        if (Slot.WeaponIcon != null)
            Slot.WeaponIcon.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            dragIcon.GetComponent<RectTransform>().position =
                eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon);
            dragIcon = null;
        }

        if (!droppedSuccessfully &&
            Slot != null)
        {
            WeaponData weapon = Slot.GetWeapon();

            if (weapon != null)
            {
                Slot.SetWeapon(weapon);
            }
        }
    }

    public WeaponData GetWeapon()
    {
        return Slot != null ? Slot.GetWeapon() : null;
    }

    public void SetDropSuccessful()
    {
        droppedSuccessfully = true;
    }
}