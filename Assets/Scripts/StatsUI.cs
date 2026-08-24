using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("Player")]
    public PlayerStats playerStats;
    public CurrencySystem currencySystem;
    public GameObject StatsPanel;
    public PlayerMovement playerMovement;

    [Header("Stats Text")]
    public TMP_Text VigorText;
    public TMP_Text EnduranceText;
    public TMP_Text StrengthText;
    public TMP_Text DexterityText;
    public TMP_Text LuckText;
    public TMP_Text FaithText;
    public TMP_Text VitalityText;

    [Header("Drived Stats Text")]
    public TMP_Text Hp;
    public TMP_Text AttackPower;
    public TMP_Text Stamina;
    public TMP_Text SkillPower;
    public TMP_Text PhysicalDef;
    public TMP_Text CritChacne;
    public TMP_Text MagicDef;
    public TMP_Text AttackSpeed;



    [Header("Level Up Buttons")]
    public Button VigorButton;
    public Button EnduranceButton;
    public Button StrengthButton;
    public Button DexterityButton;
    public Button LuckButton;
    public Button FaithButton;
    public Button VitalityButton;

    [Header("Player Info")]
    public TMP_Text LevelText;
    public TMP_Text Essence;


    [Header("EXP")]
    public TMP_Text CurrentEXP;
    public TMP_Text NextLevelEXP;


    private void Start()
    {
        Refresh();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleStats();
        }
    }

    public void Refresh()
    {

        Debug.Log("STATS UI REFRESH CALLED");

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is NULL!");
            return;
        }

        playerStats.CalculateDrivedStats();

        Debug.Log(
            $"UI VALUES | " +
            $"HP: {playerStats.HP} | " +
            $"Attack: {playerStats.AttackPower} | " +
            $"Stamina: {playerStats.Stamina} | " +
            $"SkillPower: {playerStats.SkillPower}"
        );


        if (currencySystem == null)
            return;
        UpdateEssence();

        if (playerStats == null)
            return;

        LevelText.text = playerStats.Level.ToString();

        VigorText.text = playerStats.Vigor.ToString();
        EnduranceText.text = playerStats.Endurance.ToString();
        StrengthText.text = playerStats.Strength.ToString();
        DexterityText.text = playerStats.Dexterity.ToString();
        LuckText.text = playerStats.Luck.ToString();
        FaithText.text = playerStats.Faith.ToString();
        VitalityText.text = playerStats.Vitality.ToString();


        Hp.text = playerStats.HP.ToString();
        AttackPower.text = playerStats.AttackPower.ToString();
        Stamina.text = playerStats.Stamina.ToString();
        SkillPower.text = playerStats.SkillPower.ToString();
        PhysicalDef.text = playerStats.PhysicalDef.ToString();
        CritChacne.text = playerStats.CritChacne.ToString("F1") + "%";
        MagicDef.text = playerStats.MagicDef.ToString();
        AttackSpeed.text = playerStats.AttackSpeed.ToString("F2");


        CurrentEXP.text = playerStats.EXP.ToString();
        NextLevelEXP.text = playerStats.RequiredEXP.ToString();

        UpdateButtons();

    }

    public void UpdateButtons()
    {
        bool canLevelUp = playerStats.AvailableStatPoints > 0;

        VigorButton.gameObject.SetActive(canLevelUp);
        EnduranceButton.gameObject.SetActive(canLevelUp);
        StrengthButton.gameObject.SetActive(canLevelUp);
        DexterityButton.gameObject.SetActive(canLevelUp);
        LuckButton.gameObject.SetActive(canLevelUp);
        FaithButton.gameObject.SetActive(canLevelUp);
        VitalityButton.gameObject.SetActive(canLevelUp);
    }


    public void UpdateEssence()
    {
        Essence.text = currencySystem.Essence.ToString();

    }


    public void ToggleStats()
    {
        bool isOpen = !StatsPanel.activeSelf;

        StatsPanel.SetActive(isOpen);

        if (isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerMovement.enabled = false;

            Refresh();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerMovement.enabled = true;
        }
    }
}