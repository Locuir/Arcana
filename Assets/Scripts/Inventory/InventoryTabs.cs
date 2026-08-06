using UnityEngine;
using UnityEngine.UI;

public class InventoryTabs : MonoBehaviour
{
    public GameObject CardsPanel;
    public GameObject ItemsPanel;


    public Image CardsButtonImage;
    public Image ItemsButtonImage;


    public Sprite SelectedSprite;
    public Sprite NormalSprite;


    public void ShowCards()
    {
        CardsPanel.SetActive(true);
        ItemsPanel.SetActive(false);

        CardsButtonImage.sprite = SelectedSprite;
        ItemsButtonImage.sprite = NormalSprite;

    }

    public void ShowItems()
    {
        CardsPanel.SetActive(false);
        ItemsPanel.SetActive(true);

        CardsButtonImage.sprite = NormalSprite;
        ItemsButtonImage.sprite = SelectedSprite;
    }
}