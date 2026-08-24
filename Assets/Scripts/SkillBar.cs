using UnityEngine;

public class SkillBar : MonoBehaviour
{
    public SkillSlot[] slots;
    public SkillData[] skills;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            if (i < skills.Length)
                slots[i].SetSkill(skills[i]);
            else
                slots[i].SetSkill(null);
        }
    }
}