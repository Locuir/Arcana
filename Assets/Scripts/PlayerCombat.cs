using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public Collider RightHandCollider;
    public Collider LeftHandCollider;

    private WeaponDamage currentWeaponDamage;
    private BowWeapon currentBow;

    private void Start()
    {
        RightHandCollider.enabled = false;
        LeftHandCollider.enabled = false;
    }

    public void PlaySlashEffect()
    {
        if (currentWeaponDamage == null)
        {
            return;
        }

        WeaponSlashEffect effect =
            currentWeaponDamage.GetComponentInChildren<WeaponSlashEffect>();

        if (effect == null)
        {
            return;
        }

        effect.PlaySlash();
    }

    public void ComboCheck()
    {
        playerMovement.ComboCheck();
    }

    public void SwordSlash()
    {
        AudioManager.Instance.PlaySwordSlash();
    }

    public void Footstep()
    {
        AudioManager.Instance.PlayFootstep();
    }

    public void EndAttack()
    {
        DisableRightHandCollider();
        DisableLeftHandCollider();
        DisableWeaponCollider();

        playerMovement.EndAttack();
    }

    public void EnableRightHandCollider()
    {
        HandHitbox handHitbox =
            RightHandCollider.GetComponent<HandHitbox>();

        if (handHitbox != null)
            handHitbox.ResetHits();

        RightHandCollider.enabled = true;
    }

    public void DisableRightHandCollider()
    {
        RightHandCollider.enabled = false;
    }

    public void EnableLeftHandCollider()
    {
        HandHitbox handHitbox =
            LeftHandCollider.GetComponent<HandHitbox>();

        if (handHitbox != null)
            handHitbox.ResetHits();

        LeftHandCollider.enabled = true;
    }

    public void DisableLeftHandCollider()
    {
        LeftHandCollider.enabled = false;
    }

    public void SetWeaponDamage(WeaponDamage weaponDamage)
    {
        currentWeaponDamage = weaponDamage;
    }

    public void EnableWeaponCollider()
    {
        if (currentWeaponDamage != null)
            currentWeaponDamage.EnableHitBox();
    }

    public void DisableWeaponCollider()
    {
        if (currentWeaponDamage != null)
            currentWeaponDamage.DisableHitBox();
    }

    public void SetBow(BowWeapon bow)
    {
        currentBow = bow;
    }

    public void FireProjectile()
    {
        if (currentBow != null)
            currentBow.Fire();
    }

    public void EndBowAttack()
    {
        playerMovement.animator.SetBool("IsAiming", false);
    }
}