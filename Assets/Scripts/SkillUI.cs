using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public SkillData skillData;
    private SkillManager skillManager;

    [Header("UI")]
    public Image SkillIcon;
    public Image CooldownImage;
    public TMP_Text KeyText;
    public TMP_Text CooldownText;

    private void Start()
    {
        skillManager = FindFirstObjectByType<SkillManager>();

        if (skillData != null)
        {
            SkillIcon.sprite = skillData.icon;

            if (KeyText != null)
                KeyText.text = skillData.key.ToString();
        }
    }

    private void Update()
    {
        if (skillData == null)
            return;

        if (skillManager == null)
            return;

        float cooldown =
            skillManager.GetSkillCooldownPercent(
                skillData.skillID
            );

        if (CooldownImage != null)
        {
            CooldownImage.fillAmount = cooldown;
            CooldownImage.enabled = cooldown > 0f;
        }

        if (CooldownText != null)
        {
            if (cooldown > 0f)
            {
                float remaining =
                    skillData.cooldown * cooldown;

                CooldownText.text =
                    Mathf.CeilToInt(remaining).ToString();

                CooldownText.gameObject.SetActive(true);
            }
            else
            {
                CooldownText.gameObject.SetActive(false);
            }
        }
    }
}