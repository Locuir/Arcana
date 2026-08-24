using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform rightHandHolder;
    public Transform leftHandHolder;

    public Animator animator;
    public PlayerCombat playerCombat;
    public PlayerMovement playerMovement;

    public WeaponData currentWeapon;
    public GameObject currentWeaponObject;

    public void EquipWeapon(WeaponData weapon)
    {
        if (weapon == null)
            return;

        UnequipWeapon();

        currentWeapon = weapon;

        if (playerMovement != null)
        {
            playerMovement.maxCombo = weapon.maxCombo;
        }

        Transform targetHolder =
            weapon.handType == WeaponData.HandType.RightHand
            ? rightHandHolder
            : leftHandHolder;

        if (targetHolder == null)
        {
            Debug.LogError("Target Hand Holder is not assigned!");
            return;
        }

        currentWeaponObject = Instantiate(
            weapon.weaponPrefab,
            targetHolder
        );

        currentWeaponObject.transform.localPosition = Vector3.zero;
        currentWeaponObject.transform.localRotation = Quaternion.identity;

        WeaponDamage weaponDamage =
            currentWeaponObject.GetComponentInChildren<WeaponDamage>();

        if (weaponDamage != null)
        {
            weaponDamage.WeaponData = weapon;
            playerCombat.SetWeaponDamage(weaponDamage);
        }

        BowWeapon bowWeapon =
            currentWeaponObject.GetComponentInChildren<BowWeapon>();

        if (bowWeapon != null)
        {
            bowWeapon.weaponData = weapon;
            playerCombat.SetBow(bowWeapon);
        }

        animator.runtimeAnimatorController =
            weapon.animatorOverride;

        animator.SetInteger(
            "WeaponType",
            weapon.weaponType
        );

        animator.SetTrigger("EquipWeapon");
    }

    public void UnequipWeapon()
    {
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
            currentWeaponObject = null;
        }

        if (playerCombat != null)
        {
            playerCombat.SetWeaponDamage(null);
            playerCombat.SetBow(null);
        }

        currentWeapon = null;

        animator.SetInteger("WeaponType", 0);
    }



}