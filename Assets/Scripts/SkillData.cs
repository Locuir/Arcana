using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Game/Skill")]
public class SkillData : ScriptableObject
{
    public string SkillName;
    public Sprite icon;
    public int skillID;
    public bool unlocked;

    [TextArea(2, 5)]
    public string Description;

    public WeaponData requiredWeapon;

    public float staminaCost = 20f;
    public float cooldown = 2f;

    public KeyCode key = KeyCode.None;

}