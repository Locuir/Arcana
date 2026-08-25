using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Skills")]
    public SkillData[] skills;

    [Header("Skill Logic")]
    public Dash dash;
    public PowerUp powerUp;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSkill(1);
        }
    }

    public void UseSkill(int skillID)
    {
        SkillData skill = GetSkill(skillID);

        if (skill == null)
            return;

        if (!skill.unlocked)
            return;

        switch (skillID)
        {
            case 0:

                if (dash != null)
                    dash.Activate();

                break;

            case 1:

                if (powerUp != null)
                    powerUp.Activate();

                break;
        }
    }

    private SkillData GetSkill(int skillID)
    {
        foreach (SkillData skill in skills)
        {
            if (skill != null && skill.skillID == skillID)
                return skill;
        }

        return null;
    }
}