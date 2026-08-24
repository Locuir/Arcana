using UnityEngine;

public class InventoryTabs : MonoBehaviour
{
    [Header("Pages")]
    public GameObject CardsPanel;
    public GameObject ItemsPanel;
    public GameObject StatsPanel;

    [Header("Inventory")]
    public InventoryUI inventoryUI;
    public PlayerStatsUI playerStatsUI;
    private GameObject CurrentPage;

    private void Start()
    {
        CardsPanel.SetActive(false);
        ItemsPanel.SetActive(false);
        StatsPanel.SetActive(false);

        CurrentPage = ItemsPanel;
    }

    public void ShowCards()
    {
        OpenPage(CardsPanel);
    }

    public void ShowItems()
    {
        OpenPage(ItemsPanel);
    }

    public void ShowStats()
    {
        OpenPage(StatsPanel);

        if (playerStatsUI != null)
            playerStatsUI.Refresh();
    }

    private void OpenPage(GameObject page)
    {
        if (page == null)
            return;

        if (CurrentPage != null)
            CurrentPage.SetActive(false);

        page.SetActive(true);

        CurrentPage = page;

        if (!InventoryIsOpen())
        {
            inventoryUI.OpenInventory();
        }
    }

    public void ShowCurrentPage()
    {
        if (CurrentPage == null)
        {
            OpenPage(ItemsPanel);
            return;
        }

        CurrentPage.SetActive(true);
    }

    public void HideCurrentPage()
    {
        if (CurrentPage != null)
            CurrentPage.SetActive(false);
    }

    private bool InventoryIsOpen()
    {
        return inventoryUI != null &&
               inventoryUI.InventoryPanel.activeSelf;
    }
}