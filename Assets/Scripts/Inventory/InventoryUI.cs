using UnityEngine;
using Unity.Cinemachine;
using System.Collections; 

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
    }

    private void Start()
    {
        InventoryPanel.SetActive(false);
        DarkBG.SetActive(false);

        if (ChestPanelTransform != null)
            ChestPanelTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleInventory();
    }

    public void ToggleInventory()
    {
        if (InventoryPanel.activeSelf)
            CloseInventory();
        else
            OpenInventory();
    }

    public void OpenInventory()
    {
        if (InventoryPanelTransform != null && InventoryCenterPoint != null)
        {
            InventoryPanelTransform.position = InventoryCenterPoint.position;
        }

        InventoryPanel.SetActive(true);
        DarkBG.SetActive(true);

        if (InventoryTabs != null)
            InventoryTabs.ShowCurrentPage();

        StartCoroutine(RefreshAfterFrame());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (PlayerMovement != null)
            PlayerMovement.SetCanMove(false);

        if (CameraInput != null)
            CameraInput.enabled = false;
    }

    public void OpenInventoryWithChest(Chest chest)
    {
        if (chest == null)
            return;

        if (InventoryLeftPoint != null)
        {
            InventoryPanelTransform.position = InventoryLeftPoint.position;
        }

        InventoryPanel.SetActive(true);
        DarkBG.SetActive(true);

        if (InventoryTabs != null)
            InventoryTabs.ShowCurrentPage();

        StartCoroutine(RefreshAfterFrame());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (PlayerMovement != null)
            PlayerMovement.SetCanMove(false);

        if (CameraInput != null)
            CameraInput.enabled = false;

        if (ChestUI.Instance != null)
            ChestUI.Instance.OpenChest(chest);
    }

    private IEnumerator RefreshAfterFrame()
    {
        yield return null;

        Refresh();
    }

    public void CloseInventory()
    {
        if (ChestUI.Instance != null)
            ChestUI.Instance.CloseChest();

        if (InventoryCenterPoint != null)
        {
            InventoryPanelTransform.position = InventoryCenterPoint.position;
        }

        InventoryPanel.SetActive(false);
        DarkBG.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PlayerMovement != null)
            PlayerMovement.SetCanMove(true);

        if (CameraInput != null)
            CameraInput.enabled = true;

        if (InventoryTabs != null)
            InventoryTabs.HideCurrentPage();
    }

    public void Refresh()
    {
        if (InventoryManger.Instance == null || InventoryManger.Instance.Slots == null)
            return;

        RefreshItems();
        RefreshCards();
    }

    private void RefreshItems()
    {
        if (ItemSlots == null)
            return;

        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i] == null)
                continue;

            int inventoryIndex = ItemStartIndex + i;

            ItemSlots[i].Owner = InventorySlot.SlotOwner.Inventory;
            ItemSlots[i].SlotIndex = inventoryIndex;

            if (inventoryIndex >= ItemEndIndex || inventoryIndex >= InventoryManger.Instance.Slots.Length)
            {
                ItemSlots[i].SetItem(null);
                continue;
            }

            ItemSlots[i].SetItem(InventoryManger.Instance.Slots[inventoryIndex]);
        }
    }

    private void RefreshCards()
    {
        if (CardSlots == null)
            return;

        for (int i = 0; i < CardSlots.Length; i++)
        {
            if (CardSlots[i] == null)
                continue;

            int inventoryIndex = CardStartIndex + i;

            CardSlots[i].Owner = InventorySlot.SlotOwner.Inventory;
            CardSlots[i].SlotIndex = inventoryIndex;

            if (inventoryIndex >= CardEndIndex || inventoryIndex >= InventoryManger.Instance.Slots.Length)
            {
                CardSlots[i].SetItem(null);
                continue;
            }

            CardSlots[i].SetItem(InventoryManger.Instance.Slots[inventoryIndex]);
        }
    }

    public void RefreshAll()
    {
        Refresh();

        if (ChestUI.Instance != null)
            ChestUI.Instance.Refresh();
    }
}