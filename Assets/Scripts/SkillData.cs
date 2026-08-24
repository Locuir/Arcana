using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Game/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public int skillID;
    public bool unlocked;

    public WeaponData requiredWeapon;

    public float staminaCost = 20f;
    public float cooldown = 2f;
}