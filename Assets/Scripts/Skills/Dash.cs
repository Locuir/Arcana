using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Skill")]
    public SkillData skillData;
    public int animationSkillID = 0;

    [Header("Dash")]
    public float dashDistance = 6f;
    public float dashDuration = 0.15f;
    public float dashDelay = 0.1f;

    [Header("References")]
    public StaminaSystem staminaSystem;
    public CharacterController characterController;
    public Animator animator;

    [Header("Effect")]
    public ParticleSystem dashEffect;
    public TrailRenderer dashTrail;

    private bool isDashing;
    private float cooldownTimer;

    void Start()
    {
        if (characterController == null)
            characterController = GetComponentInParent<CharacterController>();

        if (animator == null)
            animator = GetComponentInParent<Animator>();

        if (staminaSystem == null)
            staminaSystem = GetComponentInParent<StaminaSystem>();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public void Activate()
    {
        if (skillData == null)
            return;

        if (!skillData.unlocked)
            return;

        if (isDashing)
            return;

        if (cooldownTimer > 0f)
            return;

        if (staminaSystem == null)
            return;

        if (!staminaSystem.UseStamina(skillData.staminaCost))
            return;

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;

        cooldownTimer = skillData.cooldown;

        Vector3 dashDirection = transform.root.forward;
        dashDirection.y = 0f;
        dashDirection.Normalize();

        if (animator != null)
        {
            animator.SetInteger("SkillID", animationSkillID);
            animator.SetTrigger("SkillTrigger");
        }

        yield return new WaitForSeconds(dashDelay);

        if (dashEffect != null)
            dashEffect.Play();

        if (dashTrail != null)
            dashTrail.emitting = true;

        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            float distanceThisFrame =
                dashDistance / dashDuration * Time.deltaTime;

            characterController.Move(
                dashDirection * distanceThisFrame
            );

            elapsed += Time.deltaTime;

            yield return null;
        }

        if (dashTrail != null)
            dashTrail.emitting = false;

        isDashing = false;
    }

    public bool IsDashing()
    {
        return isDashing;
    }

    public float GetCooldownPercent()
    {
        if (skillData == null)
            return 0f;

        if (skillData.cooldown <= 0f)
            return 0f;

        return Mathf.Clamp01(
            cooldownTimer / skillData.cooldown
        );
    }
}