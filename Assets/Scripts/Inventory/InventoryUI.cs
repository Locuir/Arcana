using UnityEngine;
using Unity.Cinemachine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public GameObject InventoryPanel;
    public GameObject DarkBG;

    public PlayerMovement PlayerMovement;
    public WeaponAttack WeaponAttack;

    public CinemachineInputAxisController CameraInput;

    public InventoryTabs InventoryTabs;

    public InventorySlot[] CardSlots;
    public InventorySlot[] ItemSlots;

    public RectTransform InventoryPanelTransform;
    public RectTransform ChestPanelTransform;

    public RectTransform InventoryCenterPoint;
    public RectTransform InventoryLeftPoint;

    private const int ItemStartIndex = 0;
    private const int ItemEndIndex = 16;

    private const int CardStartIndex = 16;
    private const int CardEndIndex = 25;

    private void Awake()
    {
        Instance = this;

        Debug.Log("[InventoryUI] AWAKE");
    }

    private void Start()
    {
        Debug.Log("[InventoryUI] START");

        InventoryPanel.SetActive(false);
        DarkBG.SetActive(false);

        if (ChestPanelTransform != null)
            ChestPanelTransform.gameObject.SetActive(false);

        Refresh();

        Debug.Log("[InventoryUI] START COMPLETE");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("[InventoryUI] TAB PRESSED");

            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        Debug.Log(
            $"[InventoryUI] ToggleInventory | " +
            $"CurrentlyActive={InventoryPanel.activeSelf}"
        );

        if (InventoryPanel.activeSelf)
            CloseInventory();
        else
            OpenInventory();
    }

    public void OpenInventory()
    {
        Debug.Log("[InventoryUI] OPEN INVENTORY");

        if (InventoryCenterPoint != null)
        {
            InventoryPanelTransform.position =
                InventoryCenterPoint.position;
        }

        InventoryPanel.SetActive(true);
        DarkBG.SetActive(true);

        Refresh();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement.enabled = false;

        if (CameraInput != null)
            CameraInput.enabled = false;

        InventoryTabs.ShowCurrentPage();

        Debug.Log("[InventoryUI] OPEN COMPLETE");
    }

    public void OpenInventoryWithChest(Chest chest)
    {
        Debug.Log(
            $"[InventoryUI] OpenInventoryWithChest | " +
            $"Chest={(chest == null ? "NULL" : chest.name)}"
        );

        if (chest == null)
        {
            Debug.LogWarning(
                "[InventoryUI] RETURN → Chest is NULL"
            );
            return;
        }

        if (InventoryLeftPoint != null)
        {
            InventoryPanelTransform.position =
                InventoryLeftPoint.position;
        }

        InventoryPanel.SetActive(true);
        DarkBG.SetActive(true);

        Refresh();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement.enabled = false;

        if (CameraInput != null)
            CameraInput.enabled = false;

        InventoryTabs.ShowCurrentPage();

        if (ChestUI.Instance != null)
        {
            Debug.Log("[InventoryUI] Opening ChestUI");

            ChestUI.Instance.OpenChest(chest);
        }
        else
        {
            Debug.LogWarning(
                "[InventoryUI] ChestUI.Instance is NULL"
            );
        }
    }

    public void CloseInventory()
    {
        Debug.Log("[InventoryUI] CLOSE INVENTORY");

        if (ChestUI.Instance != null)
            ChestUI.Instance.CloseChest();

        if (InventoryCenterPoint != null)
        {
            InventoryPanelTransform.position =
                InventoryCenterPoint.position;
        }

        InventoryPanel.SetActive(false);
        DarkBG.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement.enabled = true;

        if (CameraInput != null)
            CameraInput.enabled = true;

        InventoryTabs.HideCurrentPage();

        Debug.Log("[InventoryUI] CLOSE COMPLETE");
    }

    public void Refresh()
    {
        Debug.Log("========== [InventoryUI] REFRESH ==========");

        if (InventoryManger.Instance == null)
        {
            Debug.LogError(
                "[InventoryUI] Refresh FAILED → InventoryManger.Instance is NULL!"
            );
            return;
        }

        if (InventoryManger.Instance.Slots == null)
        {
            Debug.LogError(
                "[InventoryUI] Refresh FAILED → Slots is NULL!"
            );
            return;
        }

        Debug.Log(
            $"[InventoryUI] Slots.Length = {InventoryManger.Instance.Slots.Length}"
        );

        RefreshItems();
        RefreshCards();

        Debug.Log("========== [InventoryUI] REFRESH COMPLETE ==========");
    }

    private void RefreshItems()
    {
        Debug.Log(
            $"[InventoryUI] RefreshItems | ItemSlots.Length={ItemSlots.Length}"
        );

        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i] == null)
            {
                Debug.LogWarning(
                    $"[InventoryUI] ItemSlots[{i}] is NULL"
                );
                continue;
            }

            int inventoryIndex =
                ItemStartIndex + i;

            Debug.Log(
                $"[InventoryUI] ITEM UI SLOT | " +
                $"UIIndex={i} | InventoryIndex={inventoryIndex}"
            );

            if (inventoryIndex >= ItemEndIndex ||
                inventoryIndex >= InventoryManger.Instance.Slots.Length)
            {
                Debug.Log(
                    $"[InventoryUI] Item UI slot {i} → EMPTY/OUT OF RANGE"
                );

                ItemSlots[i].SetItem(null);
                continue;
            }

            ItemSlots[i].Owner =
                InventorySlot.SlotOwner.Inventory;

            ItemSlots[i].SlotIndex =
                inventoryIndex;

            Debug.Log(
                $"[InventoryUI] Setting ItemSlot | " +
                $"UIIndex={i} | InventoryIndex={inventoryIndex} | " +
                $"Data={(InventoryManger.Instance.Slots[inventoryIndex] == null ? "NULL" : "HAS ITEM")}"
            );

            ItemSlots[i].SetItem(
                InventoryManger.Instance.Slots[inventoryIndex]
            );
        }
    }

    private void RefreshCards()
    {
        Debug.Log(
            $"[InventoryUI] RefreshCards | CardSlots.Length={CardSlots.Length}"
        );

        for (int i = 0; i < CardSlots.Length; i++)
        {
            if (CardSlots[i] == null)
            {
                Debug.LogWarning(
                    $"[InventoryUI] CardSlots[{i}] is NULL"
                );
                continue;
            }

            int inventoryIndex =
                CardStartIndex + i;

            Debug.Log(
                $"[InventoryUI] CARD UI SLOT | " +
                $"UIIndex={i} | InventoryIndex={inventoryIndex}"
            );

            if (inventoryIndex >= CardEndIndex ||
                inventoryIndex >= InventoryManger.Instance.Slots.Length)
            {
                Debug.Log(
                    $"[InventoryUI] Card UI slot {i} → EMPTY/OUT OF RANGE"
                );

                CardSlots[i].SetItem(null);
                continue;
            }

            CardSlots[i].Owner =
                InventorySlot.SlotOwner.Inventory;

            CardSlots[i].SlotIndex =
                inventoryIndex;

            Debug.Log(
                $"[InventoryUI] Setting CardSlot | " +
                $"UIIndex={i} | InventoryIndex={inventoryIndex} | " +
                $"Data={(InventoryManger.Instance.Slots[inventoryIndex] == null ? "NULL" : "HAS ITEM")}"
            );

            CardSlots[i].SetItem(
                InventoryManger.Instance.Slots[inventoryIndex]
            );
        }
    }

    public void RefreshAll()
    {
        Debug.Log("[InventoryUI] RefreshAll CALLED");

        Refresh();

        if (ChestUI.Instance != null)
        {
            Debug.Log("[InventoryUI] Refreshing ChestUI");

            ChestUI.Instance.Refresh();
        }
    }
}