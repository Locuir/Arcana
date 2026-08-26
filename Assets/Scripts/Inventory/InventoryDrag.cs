using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventorySlot Slot;

    private Canvas canvas;
    private GameObject dragIcon;
    private bool droppedSuccessfully;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        Debug.Log(
            $"[DRAG AWAKE] {gameObject.name} | " +
            $"Slot={(Slot != null ? Slot.name : "NULL")} | " +
            $"Canvas={(canvas != null ? canvas.name : "NULL")}"
        );
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(
            $"[DRAG BEGIN] {gameObject.name} | " +
            $"Slot={(Slot != null ? Slot.name : "NULL")} | " +
            $"Index={(Slot != null ? Slot.SlotIndex.ToString() : "NULL")} | " +
            $"Owner={(Slot != null ? Slot.Owner.ToString() : "NULL")} | " +
            $"CurrentSlot={(Slot != null && Slot.CurrentSlot != null ? "NOT NULL" : "NULL")}"
        );

        if (Slot == null)
        {
            return;
        }

        if (Slot.CurrentSlot == null)
        {
            return;
        }

        if (Slot.CurrentSlot.Item == null)
        {
            return;
        }

        droppedSuccessfully = false;

        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling();

        Image img = dragIcon.AddComponent<Image>();
        img.sprite = Slot.Icon.sprite;
        img.raycastTarget = false;

        RectTransform rect =
            dragIcon.GetComponent<RectTransform>();

        RectTransform slotRect =
            Slot.GetComponent<RectTransform>();

        rect.sizeDelta = slotRect.rect.size;
        rect.position = eventData.position;

        Slot.Icon.enabled = false;

        if (Slot.AmountText != null)
            Slot.AmountText.gameObject.SetActive(false);

        Debug.Log(
            $"[DRAG CREATED] Icon={img.sprite.name} | " +
            $"Size={rect.sizeDelta}"
        );
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
        Debug.Log(
            $"[DRAG END] " +
            $"Success={droppedSuccessfully} | " +
            $"PointerCurrent={(eventData.pointerCurrentRaycast.gameObject != null ? eventData.pointerCurrentRaycast.gameObject.name : "NULL")}"
        );

        if (dragIcon != null)
        {
            Destroy(dragIcon);
            dragIcon = null;
        }

        if (!droppedSuccessfully)
        {
            Debug.Log("[DRAG] DROP FAILED → RESTORE");

            if (Slot != null)
                Slot.SetItem(Slot.CurrentSlot);
        }
        else
        {
            Debug.Log("[DRAG] DROP SUCCESS");
        }
    }

    public void SetDropSuccessful()
    {
        droppedSuccessfully = true;

        Debug.Log(
            $"[DRAG SUCCESS SET] Slot={Slot.name} | " +
            $"Index={Slot.SlotIndex}"
        );
    }
}