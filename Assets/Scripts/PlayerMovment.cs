
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

        Debug.Log("[COMBO] PlayerMovement Started | MaxCombo = " + maxCombo);
    }

    void Update()
    {
        bool grounded = characterController.isGrounded;

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (canMove)
        {
            HandleAttackInput();
            HandleMovement();
            HandleJump(grounded);
            HandleCrouch();
        }
        else
        {
            moveDirection.x = 0f;
            moveDirection.z = 0f;

            animator.SetFloat("Velx", 0f);
            animator.SetFloat("Vely", 0f);
            animator.SetBool("Moving", false);
            animator.SetFloat("Speed", 0f);
        }

        HandleGravity(grounded);
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
                Debug.Log("[COMBO] Mouse0 Down → Attack()");
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
        Debug.Log(
            "[COMBO] Attack() | isAttacking=" +
            isAttacking +
            " | comboQueued=" +
            comboQueued +
            " | attackIndex=" +
            attackIndex
        );

        if (!isAttacking)
        {
            if (!staminaSystem.UseStamina(20f))
            {
                Debug.Log("[COMBO] Attack BLOCKED → Not enough stamina");
                return;
            }

            isAttacking = true;
            comboQueued = false;
            attackIndex = 1;

            Debug.Log("[COMBO] START ATTACK → Attack1");

            ResetAttackTriggers();
            animator.SetTrigger("Attack1");

            return;
        }

        if (attackIndex < maxCombo)
        {
            comboQueued = true;

            Debug.Log(
                "[COMBO] INPUT QUEUED | Current Attack = " +
                attackIndex +
                " | Next Attack = " +
                (attackIndex + 1)
            );
        }
        else
        {
            Debug.Log(
                "[COMBO] INPUT IGNORED → Max combo reached | attackIndex=" +
                attackIndex
            );
        }
    }

    public void ComboCheck()
    {
        Debug.Log(
            "[COMBO] ComboCheck() CALLED | isAttacking=" +
            isAttacking +
            " | comboQueued=" +
            comboQueued +
            " | attackIndex=" +
            attackIndex +
            " | maxCombo=" +
            maxCombo
        );

        if (!comboQueued || attackIndex >= maxCombo)
        {
            Debug.Log(
                "[COMBO] ComboCheck → END ATTACK | comboQueued=" +
                comboQueued +
                " | attackIndex=" +
                attackIndex
            );

            EndAttack();
            return;
        }

        if (!staminaSystem.UseStamina(20f))
        {
            Debug.Log("[COMBO] ComboCheck → END ATTACK | Not enough stamina");
            EndAttack();
            return;
        }

        comboQueued = false;
        attackIndex++;

        Debug.Log(
            "[COMBO] NEXT ATTACK → Attack" +
            attackIndex
        );

        ResetAttackTriggers();

        animator.SetTrigger(
            "Attack" + attackIndex
        );
    }

    public void EndAttack()
    {
        Debug.Log(
            "[COMBO] EndAttack() | Final attackIndex=" +
            attackIndex +
            " | comboQueued=" +
            comboQueued
        );

        isAttacking = false;
        comboQueued = false;
        attackIndex = 0;

        ResetAttackTriggers();

        animator.Play("Idle");
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
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

