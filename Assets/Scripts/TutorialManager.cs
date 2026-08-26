using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialPage
    {
        public Sprite Image;
        public string Title;
        [TextArea(2, 5)]
        public string Description;
    }

    [Header("UI")]
    public Image PageImage;
    public TMP_Text TitleText;
    public TMP_Text DescriptionText;

    public Button BackButton;
    public Button NextButton;

    [Header("Pages")]
    public TutorialPage[] Pages;

    private int currentPage;

    private void Start()
    {
        currentPage = 0;
        ShowPage();
    }

    public void NextPage()
    {
        if (currentPage < Pages.Length - 1)
        {
            currentPage++;
            ShowPage();
        }
        else
        {
            StartGame();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage();
        }
    }

    private void ShowPage()
    {
        TutorialPage page = Pages[currentPage];

        PageImage.sprite = page.Image;
        TitleText.text = page.Title;
        DescriptionText.text = page.Description;

        BackButton.gameObject.SetActive(currentPage > 0);
    }

    private void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}