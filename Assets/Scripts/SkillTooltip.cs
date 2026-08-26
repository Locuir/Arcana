using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillTooltip : MonoBehaviour
{
    public static SkillTooltip Instance;

    public GameObject Panel;
    public TMP_Text SkillNameText;
    public TMP_Text DescriptionText;
    public Image Image;

    private void Awake()
    {
        Instance = this;
        Panel.SetActive(false);
    }

    public void Show(SkillData skill)
    {
        SkillNameText.text = skill.SkillName;
        DescriptionText.text = skill.Description;
        Image.sprite = skill.icon;
        Panel.SetActive(true);
    }

    public void Hide()
    {
        Panel.SetActive(false);
    }
}