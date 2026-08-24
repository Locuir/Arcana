
using KevinIglesias;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpPower = 3f;
    public float gravity = 20f;
    public float lookSpeed = 2f;
    public float rotationSpeed = 12f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public float sprintSpeed = 5f;
    public Renderer[] Renderers;
    public StaminaSystem staminaSystem;

    private bool isAttacking;
    private bool comboQueued;
    private int attackIndex;

    public int maxCombo = 6;
    public WeaponManager weaponManager;

    private BowStringController currentBowString;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;

    private bool canMove = true;
    private bool wasGrounded;
    private Coroutine headCoroutine;

    public float headDisableDelay = 0.2f;
    public float headEnableDelay = 0.2f;

    private Camera mainCamera;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        mainCamera = Camera.main;

        wasGrounded = characterController.isGrounded;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        animator.SetInteger("WeaponType", 1);
        maxCombo = 6;
    }

    void Update()
    {
        bool grounded = characterController.isGrounded;

        if (mainCamera == null)
            mainCamera = Camera.main;

        HandleAttackInput();
        HandleMovement();
        HandleJump(grounded);
        HandleGravity(grounded);
        HandleCrouch();
        HandleCharacterMovement();
        HandleLanding();
    }

    void HandleAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canMove)
        {
            if (IsBowEquipped())
            {
                animator.ResetTrigger("Release");
                animator.SetBool("IsAiming", true);

                if (weaponManager.currentWeaponObject != null)
                {
                    currentBowString =
                        weaponManager.currentWeaponObject
                        .GetComponentInChildren<BowStringController>();
                }

                if (currentBowString != null)
                    currentBowString.LoadBow(0.3f);
                    AudioManager.Instance.PlayBowLoad();

            }
            else
            {
                Attack();
            }
        }


        if (Input.GetKeyUp(KeyCode.Mouse0) && canMove)
        {
            if (IsBowEquipped())
            {
                animator.SetTrigger("Release");
                animator.SetBool("IsAiming", false);

                if (currentBowString != null)
                    currentBowString.ShootBow(0.1f);
                    AudioManager.Instance.PlayBowRelease();
            }
        }
    }

    void HandleMovement()
    {
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);

        bool isRunning =
            wantsToRun &&
            staminaSystem.CanSprint();

        if (isRunning)
        {
            staminaSystem.UseStamina(
                staminaSystem.sprintDrain * Time.deltaTime
            );

            staminaSystem.StopRegeneration();

            if (staminaSystem.IsExhausted())
                isRunning = false;
        }
        else
        {
            staminaSystem.StartRegeneration();
        }

        float horizontalInput = canMove
            ? Input.GetAxis("Horizontal")
            : 0f;

        float verticalInput = canMove
            ? Input.GetAxis("Vertical")
            : 0f;

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 inputDirection =
            forward * verticalInput +
            right * horizontalInput;

        if (inputDirection.sqrMagnitude > 1f)
            inputDirection.Normalize();

        bool aiming =
            IsBowEquipped() &&
            animator.GetBool("IsAiming");

        if (aiming)
        {
            RotatePlayerToCrosshair();
        }
        else if (inputDirection.sqrMagnitude > 0.01f && canMove)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(inputDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        float currentSpeed = isRunning
            ? runSpeed
            : walkSpeed;

        Vector3 horizontalMovement =
            inputDirection * currentSpeed;

        moveDirection.x = horizontalMovement.x;
        moveDirection.z = horizontalMovement.z;

        Vector3 localVelocity =
            transform.InverseTransformDirection(
                new Vector3(
                    moveDirection.x,
                    0f,
                    moveDirection.z
                )
            );

        Vector2 movementInput =
            new Vector2(
                localVelocity.x,
                localVelocity.z
            );

        if (movementInput.sqrMagnitude > 1f)
            movementInput.Normalize();

        animator.SetFloat("Velx", movementInput.x);
        animator.SetFloat("Vely", movementInput.y);

        bool isMoving =
            movementInput.sqrMagnitude > 0.01f;

        animator.SetBool("Moving", isMoving);

        animator.SetFloat(
            "Speed",
            isMoving
                ? (isRunning ? 1f : 0.5f)
                : 0f
        );
    }

    void RotatePlayerToCrosshair()
    {
        if (mainCamera == null)
            return;

        Ray ray = mainCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 targetPoint;

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            1000f,
            ~0,
            QueryTriggerInteraction.Ignore))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint =
                ray.origin +
                ray.direction * 1000f;
        }

        Vector3 direction =
            targetPoint - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
    }

    void HandleJump(bool grounded)
    {
        if (!grounded)
            return;

        if (moveDirection.y < 0f)
            moveDirection.y = -2f;

        if (Input.GetButtonDown("Jump") && canMove)
        {
            moveDirection.y =
                Mathf.Sqrt(
                    jumpPower * 2f * gravity
                );

            animator.SetTrigger("Jumping");
        }
    }

    void HandleGravity(bool grounded)
    {
        if (!grounded)
        {
            moveDirection.y -=
                gravity * Time.deltaTime;
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }
    }

    void HandleCharacterMovement()
    {
        characterController.Move(
            moveDirection * Time.deltaTime
        );
    }


    void HandleLanding()
    {
        bool grounded =
            characterController.isGrounded;

        if (grounded && !wasGrounded)
        {
            if (headCoroutine != null)
                StopCoroutine(headCoroutine);

            // Re-apply the current movement state immediately
            // after landing so holding W continues into locomotion.
            bool isMoving =
                animator.GetBool("Moving");

            float speed =
                animator.GetFloat("Speed");

            animator.SetBool("Moving", isMoving);
            animator.SetFloat("Speed", speed);
        }

        wasGrounded = grounded;

        animator.SetBool("isGrounded", grounded);
    }

    public bool IsBowEquipped()
    {
        if (weaponManager == null)
            return false;

        if (weaponManager.currentWeaponObject == null)
            return false;

        return weaponManager.currentWeaponObject
            .GetComponentInChildren<BowWeapon>() != null;
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            if (!staminaSystem.UseStamina(20f))
                return;

            isAttacking = true;
            comboQueued = false;
            attackIndex = 1;

            ResetAttackTriggers();
            animator.SetTrigger("Attack1");

            return;
        }

        if (attackIndex < maxCombo)
        {
            comboQueued = true;
        }
    }

    public void ComboCheck()
    {
        if (!comboQueued || attackIndex >= maxCombo)
        {
            EndAttack();
            return;
        }

        if (!staminaSystem.UseStamina(20f))
        {
            EndAttack();
            return;
        }

        comboQueued = false;
        attackIndex++;

        ResetAttackTriggers();

        animator.SetTrigger(
            "Attack" + attackIndex
        );
    }

    public void EndAttack()
    {
        isAttacking = false;
        comboQueued = false;
        attackIndex = 0;

        ResetAttackTriggers();

        animator.Play("Idle");
    }

    private void ResetAttackTriggers()
    {
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("Attack4");
        animator.ResetTrigger("Attack5");
        animator.ResetTrigger("Attack6");
    }
}
