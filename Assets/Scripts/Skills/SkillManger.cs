using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Skills")]
    public SkillData[] skills;

    [Header("Skill Logic")]
    public Dash dash;
    public PowerUp powerUp;
    public Heal heal;
    public Invincibility invincibility;
    public Vampirism vampirism;
    public Execute execute;
    public SlowTime slowTime;
    public MadnessOfCrit madnessOfCrit;
    public BloodPrice bloodPrice;
    public ShadowMinionSkill shadowMinion;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            UseSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UseSkill(1);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            UseSkill(2);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            UseSkill(3);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            UseSkill(4);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseSkill(5);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            UseSkill(6);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            UseSkill(7);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            UseSkill(8);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            UseSkill(9);
        }
    }
    public float GetSkillCooldownPercent(int skillID)
    {
        switch (skillID)
        {
            case 0:
                return dash != null ? dash.GetCooldownPercent() : 0f;

            case 1:
                return powerUp != null ? powerUp.GetCooldownPercent() : 0f;

            case 2:
                return heal != null ? heal.GetCooldownPercent() : 0f;

            case 3:
                return invincibility != null ? invincibility.GetCooldownPercent() : 0f;

            case 4:
                return vampirism != null ? vampirism.GetCooldownPercent() : 0f;
            case 5:
                return slowTime != null ? slowTime.GetCooldownPercent() : 0f;
            case 6:
                return execute != null ? execute.GetCooldownPercent() : 0f;
            case 7:
                return madnessOfCrit != null ? madnessOfCrit.GetCooldownPercent() : 0f;
            case 8:
                return bloodPrice != null ? bloodPrice.GetCooldownPercent() : 0f;
            case 9:
                return shadowMinion != null ? shadowMinion.GetCooldownPercent() : 0f;
        }

        return 0f;
    }
    public void UseSkill(int skillID)
    {

        SkillData skill = GetSkill(skillID);

        if (skill == null)
        {
            return;
        }



        if (!skill.unlocked)
        {
            return;
        }

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

            case 2:

                if (heal != null)
                    heal.Activate();

                break;

            case 3:

                if (invincibility != null)
                    invincibility.Activate();

                break;
            case 4:

                if (vampirism != null)
                    vampirism.Activate();

                break;
            case 5:

                if (slowTime != null)
                    slowTime.Activate();

                break;
            case 6:

                if (execute != null)
                    execute.Activate();

                break;
            case 7:

                if (madnessOfCrit != null)
                    madnessOfCrit.Activate();
                break;

            case 8:

                if (bloodPrice != null)
                    bloodPrice.Activate();
                break;
            case 9:

                if (shadowMinion != null)
                    shadowMinion.Activate();
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