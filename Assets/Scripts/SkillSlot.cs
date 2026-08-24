using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Image SkillIcon;
    public SkillData skill;

    public void SetSkill(SkillData skillData)
    {
        skill = skillData;

        if (SkillIcon == null)
        {
            Debug.LogError("SkillIcon is not assigned!", gameObject);
            return;
        }

        if (skill == null)
        {
            SkillIcon.enabled = false;
            return;
        }

        if (skill.icon != null)
        {
            SkillIcon.sprite = skill.icon;
            SkillIcon.enabled = true;
        }
        else
        {
            SkillIcon.enabled = false;
        }
    }
}