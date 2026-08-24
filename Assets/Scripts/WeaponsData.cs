using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ItemData
{
    [Header("Weapon")]
    public GameObject projectilePrefab;
    public GameObject weaponPrefab;
    public int weaponType;

    [Header("Animation")]
    public AnimatorOverrideController animatorOverride;

    [Header("Combat")]
    public int damage = 10;
    public int maxCombo = 3;
    public float attackSpeed = 1f;

    [Header("Scaling")]
    [Range(0f, 2f)]
    public float strengthScaling = 1f;

    [Range(0f, 2f)]
    public float dexterityScaling = 0f;

    [Header("Hand")]
    public HandType handType;

    public enum HandType
    {
        RightHand,
        LeftHand
    }
}