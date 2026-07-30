using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Animator animator;
    public float AFKTime = 60.0f;
    float timer;
    bool CanCombo;
    public WeaponDamge CurruntWeapon;




    void Start()
    {

    }





    // Update is called once per frame
    void Update()
    {
        PlayIdleAnimation();
        PlayAttackAnimation();


    }


    void PlayIdleAnimation()
    {
        if (CheckIfAFK())
        {
            timer += Time.deltaTime;

            if (timer > AFKTime)
            {

                animator.SetBool("IsAFK", true);

            }

        }
        else
        {
            timer = 0;
            animator.SetBool("IsAFK", false);
            animator.SetTrigger("Idle");

        }



    }
    bool CheckIfAFK()
    {
        bool IsAFK = !(Keyboard.current.anyKey.isPressed || Mouse.current.delta.ReadValue() != Vector2.zero);



        return IsAFK;

    }


    void PlayAttackAnimation()
    {


        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!CanCombo)
            {

                animator.SetTrigger("Attack");

            }
            else
            {

                animator.SetTrigger("Combo");
                CanCombo = false;
                
            }




        }

    }

    public void EnableCombo() {

        CanCombo = true;
    }

    public void DisableCombo()
    {


        CanCombo = false;


    }


    public void EnableCollider()
    {
        CurruntWeapon.EnableHitBox();



    }


    public void DisableCollider()
    {
        CurruntWeapon.DisableHitBox();

    }
    
        
    


}
